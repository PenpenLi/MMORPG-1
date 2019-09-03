using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YouYou
{
    public class VarBool : Variable<bool>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarBool Alloc()
        {
            VarBool var = GameEntry.Pool.DequeueVarObject<VarBool>();
            var.Value = true;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarBool Alloc(bool value)
        {
            VarBool var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator bool(VarBool value)
        {
            return value.Value;
        }
    }
}
