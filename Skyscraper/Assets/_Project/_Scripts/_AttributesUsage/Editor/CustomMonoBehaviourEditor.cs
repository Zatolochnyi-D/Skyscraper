using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true)]
public class CustomMonoBehaviourEditor : Editor
{
    private readonly List<string> warningMessages = new();
    private IEnumerable<SerializedProperty> properties;

    protected virtual void OnEnable()
    {
        if (!EditorStyleController.UseCustomMonoBehaviourEditor)
            return;

        var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var serializableFields = fields.Where(x => x.IsPublic || x.IsDefined<SerializeField>(true));

        static bool haveRequiredAttribute(MemberInfo x) => x.IsDefined<OnThisAttribute>();
        var fieldsByAttributesPresence = serializableFields.GroupBy(haveRequiredAttribute);
        foreach (var fieldWithAttribute in fieldsByAttributesPresence.Where(x => x.Key).SelectMany(x => x))
            if (fieldWithAttribute.IsDefined<OnThisAttribute>())
                HandleOnThisAttribute(fieldWithAttribute);
        properties = fieldsByAttributesPresence.Where(x => !x.Key).SelectMany(x => x).Select(x => serializedObject.FindProperty(x.Name));
    }

    private void HandleOnThisAttribute(FieldInfo field)
    {
        if (typeof(Component).IsAssignableFrom(field.FieldType))
        {
            // Check that field object is derived from Component, that's the only type that can be attached to GameObject.
            var property = serializedObject.FindProperty(field.Name);
            if (((MonoBehaviour)target).gameObject.TryGetComponent(field.FieldType, out var component))
            {
                property.objectReferenceValue = component;
            }
            else
            {
                property.objectReferenceValue = null;
                warningMessages.Add($@"This script requires component ""{field.FieldType.Name}"" to be present on this object.");
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

    public override void OnInspectorGUI()
    {
        if (EditorStyleController.UseCustomMonoBehaviourEditor)
        {
            foreach (var message in warningMessages)
                EditorGUILayout.HelpBox(message, MessageType.Warning);
            foreach (var property in properties)
                EditorGUILayout.PropertyField(property);
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            base.OnInspectorGUI();
        }
    }
}