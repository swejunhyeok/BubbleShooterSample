using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class EditorButtonAttribute : PropertyAttribute
    {
        public string ButtonName { get; set; }
        public string MethodName { get; set; }
        public bool IsVisibleParameter { get; set; }

        public object[] Parameter = null;


        public EditorButtonAttribute(string buttonName, string methodName = null, bool isVisibleParameter = true, params object[] param)
        {
            ButtonName = buttonName;
            MethodName = methodName;
            Parameter = param;
            IsVisibleParameter = isVisibleParameter;


            if (string.IsNullOrEmpty(MethodName))
                MethodName = ButtonName;
        }
    }
}
