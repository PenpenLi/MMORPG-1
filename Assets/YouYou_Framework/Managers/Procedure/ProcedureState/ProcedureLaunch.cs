using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YouYou
{
    /// <summary>
    /// ��������
    /// </summary>
    public class ProcedureLaunch : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)//����Ƭͷ����Ȼ����ر��ⳡ��
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            //GameEntry.Procedure.ChangeState(ProcedureState.CheckVersion);
            Debug.Log("ִ����������");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
