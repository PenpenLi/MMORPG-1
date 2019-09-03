using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// YouYou�������
    /// </summary>
    public abstract class YouYouBaseComponent : YouYouComponent
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            //���Լ���������б�
            GameEntry.RegisterBaseComponent(this);
        }

        /// <summary>
        /// �رշ���
        /// </summary>
        public abstract void Shutdown();
    }
}
