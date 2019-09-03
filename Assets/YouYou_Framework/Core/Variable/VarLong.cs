using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class VarLong : Variable<long>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarLong Alloc()
        {
            VarLong var = GameEntry.Pool.DequeueVarObject<VarLong>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarLong Alloc(long value)
        {
            VarLong var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator long(VarLong value)
        {
            return value.Value;
        }
    }
}
