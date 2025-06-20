using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ExtendedCustomEditor : Editor
{
    private IEnumerable<(FieldInfo field, SerializedProperty property)> fieldPropertyPairs;
    private GameObject gameObject;

    void OnEnable()
    {
        var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var serializableFields = fields.Where(x => x.IsPublic || x.IsDefined(typeof(SerializeField), true));
        var serializedProperties = serializableFields.Select(x => serializedObject.FindProperty(x.Name));
        fieldPropertyPairs = serializableFields.Zip(serializedProperties, (x, y) => (x, y));
        gameObject = ((MonoBehaviour)target).gameObject;
    }

    private void HandleOnThisAttribute(FieldInfo field, SerializedProperty property)
    {
        if (typeof(Component).IsAssignableFrom(field.FieldType))
        {
            // Check that field object is derived from Component, that's the only type that can be attached to GameObject.
            property.objectReferenceValue = gameObject.GetComponent(field.FieldType);
        }
    }

    public override void OnInspectorGUI()
    {
        foreach (var (field, property) in fieldPropertyPairs)
        {
            if (field.IsDefined(typeof(OnThisAttribute)))
                HandleOnThisAttribute(field, property);
            else
                EditorGUILayout.PropertyField(property);
        }
        serializedObject.ApplyModifiedProperties();
    }
}