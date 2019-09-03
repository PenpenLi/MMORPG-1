using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// int����
    /// </summary>
    public class VarInt :Variable<int>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarInt Alloc()
        {
            VarInt var = GameEntry.Pool.DequeueVarObject<VarInt>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarInt Alloc(int value)
        {
            VarInt var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator int(VarInt value)
        {
            return value.Value;
        }
    }
}
