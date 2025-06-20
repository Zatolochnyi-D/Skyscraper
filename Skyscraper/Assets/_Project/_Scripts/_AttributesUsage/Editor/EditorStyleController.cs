using System;
using UnityEditor;
using UnityEngine;

public class EditorStyleController : ScriptableSingleton<EditorStyleController>
{
    [SerializeField] private bool useCustomMonoBehaviourEditor = true;

    public static bool UseCustomMonoBehaviourEditor => instance.useCustomMonoBehaviourEditor;

    [MenuItem("Custom Editor/Switch usage of custom mono behaviour editor")]
    private static void Switch() 
    {
        instance.useCustomMonoBehaviourEditor = !instance.useCustomMonoBehaviourEditor;
    }
}