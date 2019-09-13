using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace YouYou
{
    public class DataTableManager : ManagerBase
    {
        public DataTableManager()
        {
            InitDBModel();
        }

        /// <summary>
        /// �±�
        /// </summary>
        public ChapterDBModel ChapterDBModel { get; private set; }

        /// <summary>
        /// �ؿ���
        /// </summary>
        public GameLevelDBModel GameLevelDBModel { get; private set; }

        /// <summary>
        /// ��ʼ��DBModel
        /// </summary>
        private void InitDBModel()
        {
            //ÿ����new
            ChapterDBModel = new ChapterDBModel();
            GameLevelDBModel = new GameLevelDBModel();
        }

        public void LoadDataTable()
        {
            //ÿ����LoadData
            ChapterDBModel.LoadData();

            //���б�Load���
            GameEntry.Event.CommonEvent.Dispatch(SysEventId.LoadDataTableComplete);
        }

        /// <summary>
        /// �첽���ر��
        /// </summary>
        public void LoadDataTableAsync()
        {
            Task.Factory.StartNew(LoadDataTable);
        }

        public void Clear()
        {
            //ÿ����Clear
            ChapterDBModel.Clear();
        }
    }
}
