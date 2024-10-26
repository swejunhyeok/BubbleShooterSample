using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace JH
{
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    internal class ObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 기존 인스펙터 그리기 
            DrawDefaultInspector();

            // 버튼 그리기
            drawEditorButton();
        }

        private void drawEditorButton()
        {
            // 모든 메소드 정보 획득 
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            MethodInfo[] methods = target.GetType().GetMethods(flags);

            // 획득 메소드로 Inspector 출력 
            foreach (MethodInfo method in methods)
            {
                // EditorButton Attribute 획득 
                EditorButtonAttribute attribute = method.GetCustomAttribute<EditorButtonAttribute>();

                // 없는경우 버튼 생성하지 않음 
                if (attribute == null)
                    continue;

                // 버튼 생성 
                if (GUILayout.Button(attribute.ButtonName))
                {
                    // 함수 실행 
                    method.Invoke(target, attribute.Parameter);

                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
