using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class VarColor : Variable<Color>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarColor Alloc()
        {
            VarColor var = GameEntry.Pool.DequeueVarObject<VarColor>();
            var.Value = Color.clear;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarColor Alloc(Color value)
        {
            VarColor var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Color(VarColor value)
        {
            return value.Value;
        }
    }
}
