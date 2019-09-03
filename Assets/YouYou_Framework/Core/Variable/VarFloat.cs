using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class VarFloat : Variable<float>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarFloat Alloc()
        {
            VarFloat var = GameEntry.Pool.DequeueVarObject<VarFloat>();
            var.Value = 0f;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarFloat Alloc(float value)
        {
            VarFloat var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator float(VarFloat value)
        {
            return value.Value;
        }
    }
}
