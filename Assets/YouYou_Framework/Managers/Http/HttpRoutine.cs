using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace YouYou
{
    /// <summary>
    /// Http�������ݵĻص�ί��
    /// </summary>
    /// <param name="args"></param>
    public delegate void HttpSendDataCallBack(HttpCallBackArgs args);

    /// <summary>
    /// Http������
    /// </summary>
    public class HttpRoutine 
    {
        #region ����
        /// <summary>
        /// Web����ص�
        /// </summary>
        private HttpSendDataCallBack m_CallBack;

        /// <summary>
        /// Web����ص�����
        /// </summary>
        private HttpCallBackArgs m_CallBackArgs;

        /// <summary>
        /// �Ƿ�æ
        /// </summary>
        public bool IsBusy
        {
            get;
            private set;
        }
        #endregion        
        
        public HttpRoutine()
        {
            m_CallBackArgs = new HttpCallBackArgs();
        }

        #region SendData ����web����
        /// <summary>
        /// ����web����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="isPost"></param>
        /// <param name="json"></param>
        public void SendData(string url, HttpSendDataCallBack callBack, bool isPost = false, Dictionary<string, object> dic = null)
        {
            if (IsBusy) return;

            IsBusy = true;
            m_CallBack = callBack;

            if (!isPost)
            {
                GetUrl(url);
            }
            else
            {
                //web����
                if (dic != null)
                {
                    //�ͻ��˱�ʶ��
                    dic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;

                    //�豸�ͺ�
                    dic["deviceModel"] = DeviceUtil.DeviceModel;

                    long t = GameEntry.Data.SysData.CurrServerTime;
                    //ǩ��
                    dic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, DeviceUtil.DeviceIdentifier));

                    //ʱ���
                    dic["t"] = t;
                }

                string json = string.Empty;
                if (dic!=null)
                {
                    json = JsonMapper.ToJson(dic);
                    GameEntry.Pool.EnqueueClassObject(dic);
                }

                PostUrl(url, json);
            }
        }
        #endregion

        #region GetUrl Get����
        /// <summary>
        /// Get����
        /// </summary>
        /// <param name="url"></param>
        private void GetUrl(string url)
        {
            UnityWebRequest data = UnityWebRequest.Get(url);
            GameEntry.Http.StartCoroutine(Request(data));
        }
        #endregion

        #region PostUrl Post����
        /// <summary>
        /// Post����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        private void PostUrl(string url, string json)
        {
            //����һ����
            WWWForm form = new WWWForm();

            //�������ֵ
            form.AddField("", json);

            UnityWebRequest data = UnityWebRequest.Post(url,form);
            GameEntry.Http.StartCoroutine(Request(data));
        }
        #endregion

        #region Request ���������
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerator Request(UnityWebRequest data)
        {
            yield return data.SendWebRequest();

            IsBusy = false;
            if (data.isNetworkError||data.isHttpError)
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.Value = data.error;
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.downloadHandler.text;
                    m_CallBack(m_CallBackArgs);
                }
            }

            data.Dispose();
            data = null;
            GameEntry.Pool.EnqueueClassObject(this);
        }
        #endregion
    }
}
