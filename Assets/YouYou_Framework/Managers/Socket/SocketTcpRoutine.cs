using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// SocketTcp������
    /// </summary>
    public class SocketTcpRoutine
    {
        #region ������Ϣ�������
        //������Ϣ����
        private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

        //ѹ������ĳ��Ƚ���
        private const int m_CompressLen = 200;
        #endregion

        //�Ƿ����ӳɹ�
        private bool m_IsConnectedOk;

        #region ���ͽ�����Ϣ�������
        //�������ݰ����ֽ����黺����
        private byte[] m_ReceiveBuffer = new byte[1024];

        //�������ݰ��Ļ���������
        private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();

        /// <summary>
        /// �����õ�MS
        /// </summary>
        private MMO_MemoryStream m_SocketSendMS=new MMO_MemoryStream();
        /// <summary>
        /// �����õ�MS
        /// </summary>
        private MMO_MemoryStream m_SocketReceiveMS=new MMO_MemoryStream();

        //������Ϣ�Ķ���
        private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

        private int m_ReceiveCount = 0;

        /// <summary>
        /// ��һ֡�����˶���
        /// </summary>
        private int m_SendCount = 0;

        /// <summary>
        /// �Ƿ���δ������ֽ�
        /// </summary>
        private bool m_IsHasUnDealBytes = false;

        /// <summary>
        /// δ������ֽ�
        /// </summary>
        private byte[] m_UnDealBytes = null;
        #endregion

        /// <summary>
        /// �ͻ���socket
        /// </summary>
        private Socket m_Client;

        public Action OnConnectOK;

        internal void OnUpdate()
        {
            if (m_IsConnectedOk)
            {
                m_IsConnectedOk = false;
                if (OnConnectOK != null)
                {
                    OnConnectOK();
                }
                Debug.Log("���ӳɹ�");
            }

            #region �Ӷ����л�ȡ����
            while (true)
            {
                if (m_ReceiveCount <= GameEntry.Socket.MaxReceiveCount)
                {
                    m_ReceiveCount++;
                    lock (m_ReceiveQueue)
                    {
                        if (m_ReceiveQueue.Count > 0)
                        {
                            //�õ������е����ݰ�
                            byte[] buffer = m_ReceiveQueue.Dequeue();

                            //���֮�������
                            byte[] bufferNew = new byte[buffer.Length - 3];

                            bool isCompress = false;
                            ushort crc = 0;

                            MMO_MemoryStream msl = m_SocketReceiveMS;
                            msl.SetLength(0);
                            msl.Write(buffer, 0, buffer.Length);
                            msl.Position = 0;

                            isCompress = msl.ReadBool();
                            crc = msl.ReadUShort();
                            msl.Read(bufferNew, 0, bufferNew.Length);
                            //using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                            //{
                            //    isCompress = ms.ReadBool();
                            //    crc = ms.ReadUShort();
                            //    ms.Read(bufferNew, 0, bufferNew.Length);
                            //}

                            //��crc
                            int newCrc = Crc16.CalculateCrc16(bufferNew);

                            if (newCrc == crc)
                            {
                                //��� �õ�ԭʼ����
                                bufferNew = SecurityUtil.Xor(bufferNew);

                                if (isCompress)
                                {
                                    bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
                                }

                                ushort protoCode = 0;
                                byte[] protoContent = new byte[bufferNew.Length - 2];

                                MMO_MemoryStream ms2 = m_SocketReceiveMS;
                                ms2.SetLength(0);
                                ms2.Write(bufferNew, 0, bufferNew.Length);
                                ms2.Position = 0;

                                //Э����
                                protoCode = ms2.ReadUShort();
                                ms2.Read(protoContent, 0, protoContent.Length);

                                GameEntry.Event.SocketEvent.Dispatch(protoCode, protoContent);                               
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    m_ReceiveCount = 0;
                    break;
                }
            }
            #endregion

            CheckSendQueue();
        }

        #region Connect ���ӵ�socket������
        /// <summary>
        /// ���ӵ�socket������
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">�˿ں�</param>
        public void Connect(string ip, int port)
        {
            //���socket�Ѿ����� ���Ҵ���������״̬ ��ֱ�ӷ���
            if (m_Client != null && m_Client.Connected) return;

            m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_Client.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallBack, m_Client);

            }
            catch (Exception ex)
            {
                Debug.Log("����ʧ��=" + ex.Message);
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            if (m_Client.Connected)
            {
                Debug.Log("socket���ӳɹ�");
                GameEntry.Socket.RegisterSocketTcpRoutine(this);

                ReceiveMsg();
                m_IsConnectedOk = true;
            }
            else
            {
                Debug.Log("socket����ʧ��");
            }
            m_Client.EndConnect(ar);
        }
        #endregion

        /// <summary>
        /// �Ͽ�����
        /// </summary>
        public void DisConnect()
        {
            if (m_Client != null && m_Client.Connected)
            {
                m_Client.Shutdown(SocketShutdown.Both);
                m_Client.Close();
                GameEntry.Socket.RemoveSocketTcpRoutine(this);
            }
        }

        #region CheckSendQueue ��鷢�Ͷ���
        /// <summary>
        /// ��鷢�Ͷ���
        /// </summary>
        private void CheckSendQueue()
        {
            if (m_SendCount >= GameEntry.Socket.MaxSendCount)
            {
                //�ȴ���һ֡����
                m_SendCount = 0;
                return;
            }

            lock (m_SendQueue)
            {
                if (m_SendQueue.Count > 0 || m_IsHasUnDealBytes)
                {
                    MMO_MemoryStream ms = m_SocketSendMS;
                    ms.SetLength(0);

                    //�ȴ���δ����İ�
                    if (m_IsHasUnDealBytes)
                    {
                        m_IsHasUnDealBytes = false;
                        ms.Write(m_UnDealBytes, 0, m_UnDealBytes.Length);
                    }

                    while (true)
                    {
                        if(m_SendQueue.Count == 0)
                        {
                            break;
                        }

                        //ȡ��һ���ֽ�����
                        byte[] buffer = m_SendQueue.Dequeue();

                        if ((buffer.Length+ms.Length)<=GameEntry.Socket.MaxSendByteCount)
                        {
                            ms.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            //�Ѿ�ȡ����һ��Ҫ���͵��ֽ�����
                            m_UnDealBytes = buffer;
                            m_IsHasUnDealBytes = true;
                            break;
                        }
                    }

                    Send(ms.ToArray());
                }
            }
        }
        #endregion

        #region MakeData ��װ���ݰ�
        /// <summary>
        /// ��װ���ݰ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] MakeData(byte[] data)
        {
            byte[] retBuffer = null;

            //1.������ݰ��ĳ��� ������m_CompressLen �����ѹ��
            bool isCompress = data.Length > m_CompressLen ? true : false;
            if (isCompress)
            {
                data = ZlibHelper.CompressBytes(data);
            }

            //2.���
            data = SecurityUtil.Xor(data);

            //3.CrcУ�� ѹ�����
            ushort crc = Crc16.CalculateCrc16(data);

            MMO_MemoryStream ms = m_SocketSendMS;
            ms.SetLength(0);

            ms.WriteUShort((ushort)(data.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(data, 0, data.Length);

            retBuffer = ms.ToArray();
            return retBuffer;
        }
        #endregion

        #region SendMsg ������Ϣ ����Ϣ�������
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="buffer"></param>
        public void SendMsg(byte[] buffer)
        {
            //�õ���װ������ݰ�
            byte[] sendBuffer = MakeData(buffer);

            lock (m_SendQueue)
            {
                //�����ݰ��������
                m_SendQueue.Enqueue(sendBuffer);
            }
        }
        #endregion

        #region Send �����������ݰ���������
        /// <summary>
        /// �����������ݰ���������
        /// </summary>
        /// <param name="buffer"></param>
        private void Send(byte[] buffer)
        {
            m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
        }
        #endregion

        #region SendCallBack �������ݰ��Ļص�
        /// <summary>
        /// �������ݰ��Ļص�
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            m_Client.EndSend(ar);
        }
        #endregion

        //====================================================

        #region ReceiveMsg ��������
        /// <summary>
        /// ��������
        /// </summary>
        private void ReceiveMsg()
        {
            //�첽��������
            m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
        }
        #endregion

        #region ReceiveCallBack �������ݻص�
        /// <summary>
        /// �������ݻص�
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int len = m_Client.EndReceive(ar);

                if (len > 0)
                {
                    //�Ѿ����յ�����

                    //�ѽ��յ����� д�뻺����������β��
                    m_ReceiveMS.Position = m_ReceiveMS.Length;

                    //��ָ�����ȵ��ֽ� д��������
                    m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                    //��������������ĳ���>2 ˵�������и��������İ�������
                    //Ϊʲô������2 ��Ϊ���ǿͻ��˷�װ���ݰ� �õ�ushort ���Ⱦ���2
                    if (m_ReceiveMS.Length > 2)
                    {
                        //����ѭ�� ������ݰ�
                        while (true)
                        {
                            //��������ָ��λ�÷���0��
                            m_ReceiveMS.Position = 0;

                            //currMsgLen = ����ĳ���
                            int currMsgLen = m_ReceiveMS.ReadUShort();

                            //currFullMsgLen �ܰ��ĳ���=��ͷ����+���峤��
                            int currFullMsgLen = 2 + currMsgLen;

                            //����������ĳ���>=�����ĳ��� ˵�������յ���һ��������
                            if (m_ReceiveMS.Length >= currFullMsgLen)
                            {
                                //�����յ�һ��������

                                //��������byte[]����
                                byte[] buffer = new byte[currMsgLen];

                                //��������ָ��ŵ�2��λ�� Ҳ���ǰ����λ��
                                m_ReceiveMS.Position = 2;

                                //�Ѱ������byte[]����
                                m_ReceiveMS.Read(buffer, 0, currMsgLen);

                                lock (m_ReceiveQueue)
                                {
                                    m_ReceiveQueue.Enqueue(buffer);
                                }
                                //==============����ʣ���ֽ�����===================

                                //ʣ���ֽڳ���
                                int remainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                                if (remainLen > 0)
                                {
                                    //��ָ����ڵ�һ������β��
                                    m_ReceiveMS.Position = currFullMsgLen;

                                    //����ʣ���ֽ�����
                                    byte[] remainBuffer = new byte[remainLen];

                                    //������������ʣ���ֽ�����
                                    m_ReceiveMS.Read(remainBuffer, 0, remainLen);

                                    //���������
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    //��ʣ���ֽ���������д��������
                                    m_ReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                    remainBuffer = null;
                                }
                                else
                                {
                                    //û��ʣ���ֽ�

                                    //���������
                                    m_ReceiveMS.Position = 0;
                                    m_ReceiveMS.SetLength(0);

                                    break;
                                }
                            }
                            else
                            {
                                //��û���յ�������
                                break;
                            }
                        }
                    }

                    //������һ�ν������ݰ�
                    ReceiveMsg();
                }
                else
                {
                    //�ͻ��˶Ͽ�����
                    Debug.Log(string.Format("������{0}�Ͽ�����", m_Client.RemoteEndPoint.ToString()));
                }
            }
            catch
            {
                //�ͻ��˶Ͽ�����
                Debug.Log(string.Format("������{0}�Ͽ�����", m_Client.RemoteEndPoint.ToString()));
            }
        }
        #endregion
    }
}
