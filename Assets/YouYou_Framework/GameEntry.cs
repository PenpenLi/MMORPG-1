using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// ��Ϸ�����
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        #region �������
        /// <summary>
        /// �¼����
        /// </summary>
        public static EventComponent Event
        {
            get;
            private set;
        }

        /// <summary>
        /// ʱ�����
        /// </summary>
        public static TimeComponent Time
        {
            get;
            private set;
        }

        /// <summary>
        /// ״̬�����
        /// </summary>
        public static FsmComponent Fsm
        {
            get;
            private set;
        }

        /// <summary>
        /// �������
        /// </summary>
        public static ProcedureComponent Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// ���ݱ����
        /// </summary>
        public static DataTableComponent DataTable
        {
            get;
            private set;
        }

        /// <summary>
        /// Socket���
        /// </summary>
        public static SocketComponent Socket
        {
            get;
            private set;
        }

        /// <summary>
        /// Http���
        /// </summary>
        public static HttpComponent Http
        {
            get;
            private set;
        }

        /// <summary>
        /// �������
        /// </summary>
        public static DataComponent Data
        {
            get;
            private set;
        }

        public static LocalizationComponent Localization
        {
            get;
            private set;
        }

        public static PoolComponent Pool
        {
            get;
            private set;
        }

        public static SceneComponent Scene
        {
            get;
            private set;
        }

        public static SettingComponent Setting
        {
            get;
            private set;
        }

        public static GameObjComponent GameObj
        {
            get;
            private set;
        }

        public static ResourceComponent Resource
        {
            get;
            private set;
        }

        public static DownloadComponent Download
        {
            get;
            private set;
        }

        /// <summary>
        /// UI���
        /// </summary>
        public static UIComponent UI
        {
            get;
            private set;
        }
        #endregion

        #region �����������
        /// <summary>
        /// ����������б�
        /// </summary>
        private static readonly LinkedList<YouYouBaseComponent> m_BaseComponentList = new LinkedList<YouYouBaseComponent>();

        #region ע�����
        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="component"></param>
        internal static void RegisterBaseComponent(YouYouBaseComponent component)
        {
            //��ȡ���������
            Type type = component.GetType();

            LinkedListNode<YouYouBaseComponent> curr = m_BaseComponentList.First;
            while (curr!=null)
            {
                if (curr.Value.GetType() == type) return;
                curr = curr.Next;
            }

            //������������һ���ڵ�
            m_BaseComponentList.AddLast(component);
        }
        #endregion

        #region ��ȡ�������
        internal static T GetBaseComponent<T>() where T : YouYouBaseComponent
        {
            return (T)GetBaseComponent(typeof(T));
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="component"></param>
        internal static YouYouBaseComponent GetBaseComponent(Type type)
        {     
            LinkedListNode<YouYouBaseComponent> curr = m_BaseComponentList.First;
            while (curr != null)
            {
                if (curr.Value.GetType() == type)
                {
                    return curr.Value;
                }
                curr = curr.Next;
            }
            return null;
        }
        #endregion
        #endregion

        #region �����������
        /// <summary>
        /// ����������б�
        /// </summary>
        private static readonly LinkedList<IUpdateComponent> m_UpdateComponentList = new LinkedList<IUpdateComponent>();

        #region ע��������
        /// <summary>
        /// ע��������
        /// </summary>
        /// <param name="component"></param>
        public static void RegisterUpdateComponent(IUpdateComponent component)
        {
            //������������һ���ڵ�
            m_UpdateComponentList.AddLast(component);
        }
        #endregion

        #region �Ƴ��������
        /// <summary>
        /// �Ƴ��������
        /// </summary>
        /// <param name="component"></param>
        public static void RemoveUpdateComponent(IUpdateComponent component)
        {
           m_UpdateComponentList.Remove(component);      
        }
        #endregion
        #endregion

        void Start()
        {
            InitBaseComponents();
        }

        private static void InitBaseComponents()
        {
            Event = GetBaseComponent<EventComponent>();
            Time = GetBaseComponent<TimeComponent>();
            Fsm = GetBaseComponent<FsmComponent>();
            Procedure = GetBaseComponent<ProcedureComponent>();
            DataTable = GetBaseComponent<DataTableComponent>();
            Socket = GetBaseComponent<SocketComponent>();
            Http = GetBaseComponent<HttpComponent>();
            Data = GetBaseComponent<DataComponent>();
            Localization = GetBaseComponent<LocalizationComponent>();
            Pool = GetBaseComponent<PoolComponent>();
            Scene = GetBaseComponent<SceneComponent>();
            Setting = GetBaseComponent<SettingComponent>();
            GameObj = GetBaseComponent<GameObjComponent>();
            Resource = GetBaseComponent<ResourceComponent>();
            Download = GetBaseComponent<DownloadComponent>();
            UI = GetBaseComponent<UIComponent>();
        }

        void Update()
        {
            //ѭ���������
            for(LinkedListNode<IUpdateComponent> curr = m_UpdateComponentList.First; curr != null; curr = curr.Next)
            {
                curr.Value.OnUpdate();
            }
        }

        private void OnDestroy()
        {
            //�ر����л������
            for (LinkedListNode<YouYouBaseComponent> curr = m_BaseComponentList.First; curr != null; curr = curr.Next)
            {
                curr.Value.Shutdown();
            }
        }
    }
}

