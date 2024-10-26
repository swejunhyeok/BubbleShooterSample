using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JH
{
    public class FoldScope : System.IDisposable
    {
        public FoldScope(ref bool isFold, string name = "", GUIStyle style = null)
        {
            if (style == null)
            {
                style = new GUIStyle(EditorStyles.foldoutHeader);
                style.onActive.background = style.hover.background;
            }

            Rect foldRect = EditorGUILayout.GetControlRect();
            isFold = EditorGUI.BeginFoldoutHeaderGroup(foldRect, isFold, name, style);

            EditorGUI.indentLevel++;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel--;
            EditorGUI.EndFoldoutHeaderGroup();
        }
    }
}
