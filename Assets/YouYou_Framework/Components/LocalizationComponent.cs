using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public enum YouYouLanguage
    {
        /// <summary>
        /// ����
        /// </summary>
        Chinese=0,
        /// <summary>
        /// Ӣ��
        /// </summary>
        English=1
    }

    /// <summary>
    /// ���ػ����
    /// </summary>
    public class LocalizationComponent : YouYouBaseComponent
    {
        /// <summary>
        /// ��ǰ����(Ҫ�ͱ��ػ���������ֶ� һ��)
        /// </summary>
        public YouYouLanguage CurrLanguage
        {
            get;
            private set;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                case SystemLanguage.Chinese:
                    CurrLanguage = YouYouLanguage.Chinese;
                    break;
                case SystemLanguage.English:
                    CurrLanguage = YouYouLanguage.English;
                    break;
            }
        }

        public override void Shutdown()
        {

        }

    }
}
