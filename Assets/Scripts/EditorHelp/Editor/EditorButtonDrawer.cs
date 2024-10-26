using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JH
{
    [CustomPropertyDrawer(typeof(EditorButtonAttribute))]
    public class EditorButtonDrawer : PropertyDrawer
    {
        private MethodInfo _eventMethodInfo = null;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return base.CreatePropertyGUI(property);
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorButtonAttribute editorButton = (EditorButtonAttribute)attribute;

            string path = property.propertyPath;
            int arrayInd = path.LastIndexOf(".Array");
            bool bIsArray = arrayInd >= 0;


            if (bIsArray)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                if (!editorButton.IsVisibleParameter)
                {
                    if (GUI.Button(position, editorButton.ButtonName))
                    {
                        System.Type eventOwnerType = property.serializedObject.targetObject.GetType();
                        string eventName = editorButton.MethodName;
                        if (_eventMethodInfo == null)
                            _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                        if (_eventMethodInfo != null)
                        {
                            _eventMethodInfo.Invoke(property.serializedObject.targetObject, editorButton.Parameter);

                            EditorUtility.SetDirty(property.serializedObject.targetObject);
                        }
                        else
                            Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
                    }
                }
                else
                {
                    float height = EditorGUI.GetPropertyHeight(property);

                    position.height = 16;

                    label.text = property.displayName;
                    EditorGUI.PropertyField(position, property, label);

                    if (!string.IsNullOrEmpty(editorButton.MethodName))
                    {
                        position.position += new Vector2(0, height);

                        if (GUI.Button(position, editorButton.ButtonName))
                        {
                            System.Type eventOwnerType = property.serializedObject.targetObject.GetType();
                            string eventName = editorButton.MethodName;
                            if (_eventMethodInfo == null)
                                _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                            if (_eventMethodInfo != null)
                            {
                                _eventMethodInfo.Invoke(property.serializedObject.targetObject, editorButton.Parameter);

                                EditorUtility.SetDirty(property.serializedObject.targetObject);
                            }
                            else
                                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
                        }
                    }
                }
            }


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            EditorButtonAttribute editorButton = (EditorButtonAttribute)attribute;

            string path = property.propertyPath;
            int arrayInd = path.LastIndexOf(".Array");
            bool bIsArray = arrayInd >= 0;

            float expandHeight = 0;

            if (!bIsArray && !string.IsNullOrEmpty(editorButton.MethodName) && editorButton.IsVisibleParameter)
            {
                expandHeight = 18;
            }

            return base.GetPropertyHeight(property, label) + expandHeight;
        }
    }
}
