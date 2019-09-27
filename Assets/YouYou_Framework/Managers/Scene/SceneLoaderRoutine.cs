using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YouYou
{
    /// <summary>
    /// �������غ�ж����
    /// </summary>
    public class SceneLoaderRoutine
    {
        private AsyncOperation m_CurrAsync = null;

        /// <summary>
        /// ���ȸ���
        /// </summary>
        private BaseAction<int, float> OnProgressUpdate;

        /// <summary>
        /// ���س������
        /// </summary>
        private BaseAction<SceneLoaderRoutine> OnLoadSceneComplete;

        /// <summary>
        /// ж�س������
        /// </summary>
        private BaseAction<SceneLoaderRoutine> OnUnLoadSceneComplete;

        /// <summary>
        /// ������ϸ���
        /// </summary>
        private int m_SceneDetailId;

        public void LoadScene(int sceneDetailId,string sceneName,BaseAction<int,float> onProgressUpdate,BaseAction<SceneLoaderRoutine> onLoadSceneComplete)
        {
            Reset();

            m_SceneDetailId = sceneDetailId;
            OnProgressUpdate = onProgressUpdate;
            OnLoadSceneComplete = onLoadSceneComplete;

            //GameEntry.Resource.res
        }

        private void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
