using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class UIManager : ManagerBase
    {
        /// <summary>
        /// ��UI����
        /// </summary>
        /// <param name="uiFormId"></param>
        /// <param name="userData"></param>
        internal void OpenUIForm(int uiFormId,object userData=null)
        {
            //1.����
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR

            //Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
#else

#endif
        }
    }
}
