using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class VarString : Variable<string>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarString Alloc()
        {
            VarString var = GameEntry.Pool.DequeueVarObject<VarString>();
            var.Value = "";
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarString Alloc(string value)
        {
            VarString var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator string(VarString value)
        {
            return value.Value;
        }
    }
}
