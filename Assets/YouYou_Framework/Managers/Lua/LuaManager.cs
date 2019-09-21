using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace YouYou
{
    /// <summary>
    /// Lua������
    /// </summary>
    public class LuaManager : ManagerBase
    {
        /// <summary>
        /// ȫ�ֵ�xLua����
        /// </summary>
        public static LuaEnv luaEnv;

        public void Init()
        {
            //1.ʵ���� xLua����
            luaEnv = new LuaEnv();

#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
            //2.����xLua�Ľű�·��
            luaEnv.DoString(string.Format("package.path = '{0}/?.bytes'", Application.dataPath+ "Download/xLuaLogic/"));
#else
            luaEnv.AddLoader(MyLoader);
            //luaEnv.DoString(string.Format("package.path = '{0}/?.lua'", Application.persistentDataPath));
#endif
            DoString("require'Main'");
        }

        /// <summary>
        /// �Զ����Loader
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private byte[] MyLoader(ref string filepath)
        {
            //string path = Application.persistentDataPath + "/" + filepath + ".lua";
            byte[] buffer = null;
            //using (FileStream fs = new FileStream(path, FileMode.Open))
            //{
            //    buffer = new byte[fs.Length];
            //    fs.Read(buffer, 0, buffer.Length);
            //}
            return buffer;
        }

        /// <summary>
        /// ִ��lua�ű�
        /// </summary>
        /// <param name="str"></param>
        public void DoString(string str)
        {
            luaEnv.DoString(str);
        }
    }
}
