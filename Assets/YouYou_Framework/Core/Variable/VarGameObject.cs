using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class VarGameObject : Variable<GameObject>
    {
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <returns></returns>
        public static VarGameObject Alloc()
        {
            VarGameObject var = GameEntry.Pool.DequeueVarObject<VarGameObject>();
            var.Value = null;
            var.Retain();
            return var;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="value">��ʼֵ</param>
        /// <returns></returns>
        public static VarGameObject Alloc(GameObject value)
        {
            VarGameObject var = Alloc();
            var.Value = value;
            return var;
        }

        /// <summary>
        /// VarInt->int
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator GameObject(VarGameObject value)
        {
            return value.Value;
        }
    }
}
