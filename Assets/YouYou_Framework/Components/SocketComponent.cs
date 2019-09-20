using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// Socket���
    /// </summary>
    public class SocketComponent : YouYouBaseComponent,IUpdateComponent
    {
        [Header("ÿ֡���������")]
        public int MaxSendCount=5;

        [Header("ÿ�η��������ֽ�")]
        public int MaxSendByteCount = 1024;

        [Header("ÿ֡����������")]
        public int MaxReceiveCount = 5;

        private SocketManager m_SocketManager;

        ///// <summary>
        ///// ͨ�õ�MemoryStream
        ///// </summary>
        //public MMO_MemoryStream CommonMemoryStream
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// �����õ�MS
        /// </summary>
        public MMO_MemoryStream SocketSendMS
        {
            get;
            private set;
        }

        /// <summary>
        /// �����õ�MS
        /// </summary>
        public MMO_MemoryStream SocketReceiveMS
        {
            get;
            private set;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            m_SocketManager = new SocketManager();
            SocketSendMS = new MMO_MemoryStream();
            SocketReceiveMS = new MMO_MemoryStream();
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_MainSocket = CreateSocketTcpRoutine();
        }

        /// <summary>
        /// ע��SocketTcp������
        /// </summary>
        /// <param name="routine"></param>
        internal void RegisterSocketTcpRoutine(SocketTcpRoutine routine)
        {
            m_SocketManager.RegisterSocketTcpRoutine(routine);
        }

        /// <summary>
        /// �Ƴ�SocketTcp������
        /// </summary>
        /// <param name="routine"></param>
        internal void RemoveSocketTcpRoutine(SocketTcpRoutine routine)
        {
            m_SocketManager.RemoveSocketTcpRoutine(routine);
        }

        /// <summary>
        /// ����SocketTcp������
        /// </summary>
        /// <returns></returns>
        public SocketTcpRoutine CreateSocketTcpRoutine()
        {
            return GameEntry.Pool.DequeueClassObject<SocketTcpRoutine>();
        }

        public override void Shutdown()
        {
            m_SocketManager.Dispose();

            GameEntry.Pool.EnqueueClassObject(m_MainSocket);

            SocketSendMS.Dispose();
            SocketReceiveMS.Dispose();

            SocketSendMS.Close();
            SocketReceiveMS.Close();
        }

        public void OnUpdate()
        {
            m_SocketManager.OnUpdate();
        }

        //===============================================
        /// <summary>
        /// ��Socket
        /// </summary>
        private SocketTcpRoutine m_MainSocket;

        /// <summary>
        /// ���ӵ���Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void ConnectToMainSocket(string ip,int port)
        {
            m_MainSocket.Connect(ip, port);
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="buffer"></param>
        public void SendMsg(byte[] buffer)
        {
            m_MainSocket.SendMsg(buffer);
        }       
    }
}
