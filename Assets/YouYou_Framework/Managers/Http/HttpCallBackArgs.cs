using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class HttpCallBackArgs : EventArgs
    {
        /// <summary>
        /// �Ƿ��д�
        /// </summary>
        public bool HasError;

        /// <summary>
        /// ����ֵ
        /// </summary>
        public string Value;
    }
}
