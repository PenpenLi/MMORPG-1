using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// �¼�������
    /// </summary>
    public class EventManager : ManagerBase, IDisposable
    {
        public SocketEvent SocketEvent
        {
            private set;
            get;
        }

        public CommonEvent CommonEvent
        {
            private set;
            get;
        }

        public EventManager()
        {
            SocketEvent = new SocketEvent();
            CommonEvent = new CommonEvent();
        }
        
        public void Dispose()
        {
            SocketEvent.Dispose();
            CommonEvent.Dispose();
        }
    }
}
