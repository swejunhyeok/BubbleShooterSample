using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JH
{
    public class BoxScope : System.IDisposable
    {
        GUIStyle boxScopeStyle;
        GUIStyle BoxScopeStyle
        {
            get
            {
                if (boxScopeStyle == null)
                {
                    boxScopeStyle = new GUIStyle(EditorStyles.helpBox);
                    RectOffset p = boxScopeStyle.padding;
                    p.right += 6;
                    p.top += 1;
                    p.left += 6;
                    p.bottom += 1;

                    RectOffset m = BoxScopeStyle.margin;
                    m.right = 2;
                    m.left = 2;
                    m.top = 2;
                    m.bottom = 2;
                }

                return boxScopeStyle;
            }
        }

        public BoxScope(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(BoxScopeStyle, options);

            EditorGUILayout.Space(5);
        }

        public void Dispose()
        {
            EditorGUILayout.Space(5);

            EditorGUILayout.EndVertical();
        }
    }
}
