using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// Byte�������
    /// </summary>
    public class VarBytes : Variable<byte[]>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarBytes Alloc()
        {
            VarBytes var = GameEntry.Pool.DequeueVarObject<VarBytes>();
            var.Value = null;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarBytes Alloc(byte[] value)
        {
            VarBytes var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator byte[](VarBytes value)
        {
            return value.Value;
        }
    }
}
