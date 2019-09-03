using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// byte����
    /// </summary>
    public class VarByte : Variable<byte>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarByte Alloc()
        {
            VarByte var = GameEntry.Pool.DequeueVarObject<VarByte>();
            var.Value = 0;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarByte Alloc(byte value)
        {
            VarByte var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->byte
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator byte(VarByte value)
        {
            return value.Value;
        }
    }
}
