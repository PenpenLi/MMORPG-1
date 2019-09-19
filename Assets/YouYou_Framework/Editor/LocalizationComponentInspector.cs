using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YouYou
{
    [CustomEditor(typeof(LocalizationComponent), true)]
    public class LocalizationComponentInspector : Editor
    {
        private SerializedProperty m_CurrLanguage = null;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_CurrLanguage);
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            //�������Թ�ϵ
            m_CurrLanguage = serializedObject.FindProperty("m_CurrLanguage");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
