using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace YouYou
{
    /// <summary>
    /// Lua����
    /// </summary>
    [LuaCallCSharp]
    public class LuaForm :UIFormBase
    {
        [CSharpCallLua]
        public delegate void OnInitHandler(object userData);
        OnInitHandler onInit;

        [CSharpCallLua]
        public delegate void OnOpenHandler(object userData);
        OnOpenHandler onOpen;

        [CSharpCallLua]
        public delegate void OnCloseHandler();
        OnCloseHandler onClose;

        [CSharpCallLua]
        public delegate void OnBeforDestroyHandler();
        OnBeforDestroyHandler onBeforDestroy;

        private LuaTable scriptEnv;
        private LuaEnv luaEnv;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            luaEnv = LuaManager.luaEnv; //�˴�Ҫ��LuaManager�ϻ�ȡ ȫ��ֻ��һ��

            scriptEnv = luaEnv.NewTable();

            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            string prefabName = name;
            if (prefabName.Contains("(Clone)"))
            {
                prefabName = prefabName.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0]+"View";
            }

            onInit = scriptEnv.GetInPath<OnInitHandler>(prefabName + ".OnInit");
            onOpen = scriptEnv.GetInPath<OnOpenHandler>(prefabName + ".OnOpen");
            onClose = scriptEnv.GetInPath<OnCloseHandler>(prefabName + ".OnClose");
            onBeforDestroy = scriptEnv.GetInPath<OnBeforDestroyHandler>(prefabName + ".OnBeforDestroy");

            scriptEnv.Set("self", this);
            if (onInit != null)
            {
                onInit(userData);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (onOpen != null)
            {
                onOpen(userData);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (onClose != null)
            {
                onClose();
            }
        }

        protected override void OnBeforDestroy()
        {
            base.OnBeforDestroy();
            //��ע �������ٵĻ������������Unity����
            if (onBeforDestroy != null)
            {
                onBeforDestroy();
            }
            onInit = null;
            onOpen = null;
            onClose = null;
            onBeforDestroy = null;
        }
    }
}

