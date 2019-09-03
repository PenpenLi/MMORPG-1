using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// ��������
    /// </summary>
    public abstract class VariableBase 
    {
        public abstract Type Type
        {
            get;
        }

        public byte ReferenceCount
        {
            get;
            private set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void Retain()
        {
            ReferenceCount++;
        }

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        public void Release()
        {
            ReferenceCount--;
            if (ReferenceCount<1)
            {
                //�سز���
                GameEntry.Pool.EnqueueVarObject(this);
            }
        }
    }
}
