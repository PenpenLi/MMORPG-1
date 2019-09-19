using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// �û�����
    /// </summary>
    public class UserDataManager : IDisposable
    {
        /// <summary>
        /// ���������������б�
        /// </summary>
        public List<ServerTaskEntity> ServerTaskList
        {
            get;
            private set;
        }
        public UserDataManager()
        {
            ServerTaskList = new List<ServerTaskEntity>();
        }

        public void Clear()
        {
            ServerTaskList.Clear();
        }

        public void Dispose()
        {
            ServerTaskList.Clear();
        }

        public void ReceiveTask(Task_SearchTaskReturnProto proto)
        {
            int len = proto.CurrTaskItemList.Count;
            for (int i=0;i<len;i++)
            {
                Task_SearchTaskReturnProto.TaskItem item = proto.CurrTaskItemList[i];
                ServerTaskList.Add(new ServerTaskEntity()
                {
                    Id = item.Id,
                    Status = item.Status
                });
            }           
        }
    }
}
