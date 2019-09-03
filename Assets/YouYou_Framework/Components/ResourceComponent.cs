using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// ��Դ���
    /// </summary>
    public class ResourceComponent : YouYouBaseComponent
    {
        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public string LocalFilePath;

        protected override void OnAwake()
        {
            base.OnAwake();

#if DISABLE_ASSETBUNDLE
            LocalFilePath = Application.dataPath;
#else
            LocalFilePath = Application.persistentDataPath;
#endif
        }

        /// <summary>
        /// ��ȡ�����ļ���byte����
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] GetFileBuffer(string path)
        {
            byte[] buffer = null;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        public override void Shutdown()
        {

        }
    }
}
