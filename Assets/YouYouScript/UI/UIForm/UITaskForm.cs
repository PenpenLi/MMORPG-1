using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YouYou;

public class UITaskForm : UIFormBase
{
    /// <summary>
    /// �����б�
    /// </summary>
    [SerializeField]
    private UIMultiScroller multiScroller;

    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    private Text txtTaskName;

    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    private Text txtTaskDesc;

    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    private Text txtAwardMoney;

    /// <summary>
    /// ��ȡ��ť
    /// </summary>
    [SerializeField]
    private Button btnReceive;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        //��ȡ��ť���
        btnReceive.onClick.AddListener(()=> 
        {


        });

        multiScroller.OnItemCreate = OnItemCreate;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //���������б�
        LoadTaskList();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void OnBeforDestroy()
    {
        base.OnBeforDestroy();

        multiScroller = null;
        txtTaskName = null;
        txtTaskDesc = null;
        txtAwardMoney = null;
        btnReceive = null;
    }

    //private List<TaskEntity> m_TaskListTable;
    private List<ServerTaskEntity> m_ServerTaskList;

    /// <summary>
    /// ���������б�
    /// </summary>
    private void LoadTaskList()
    {
        //m_TaskListTable = GameEntry.DataTable.DataTableManager.TaskDBModel.GetList();
        m_ServerTaskList = GameEntry.Data.UserDataManager.ServerTaskList;

        //multiScroller.DataCount = m_TaskListTable.Count;
        multiScroller.DataCount = m_ServerTaskList.Count;
        multiScroller.ResetScroller();

        //OnBtnDetailClick(m_TaskListTable[0].Id);
        OnBtnDetailClick(m_ServerTaskList[0].Id);
    }

    private void OnItemCreate(int index, GameObject obj)
    {
        UITaskFormItemView view = obj.GetComponent<UITaskFormItemView>();
        //view.SetUI(m_TaskListTable[index], OnBtnDetailClick);
        view.SetUI(m_ServerTaskList[index], OnBtnDetailClick);
    }

    private void OnBtnDetailClick(int id)
    {
        TaskEntity entity = GameEntry.DataTable.DataTableManager.TaskDBModel.Get(id);
        txtTaskName.text = GameEntry.Localization.GetString(entity.Name);
        txtTaskDesc.text = GameEntry.Localization.GetString(entity.Content);
        txtAwardMoney.text = "100";
    } 
}
