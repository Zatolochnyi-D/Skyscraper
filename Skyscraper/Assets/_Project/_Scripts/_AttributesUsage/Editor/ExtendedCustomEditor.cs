using System.Reflection;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ExtendedCustomMonoBehaviourEditor : Editor
{
    protected GameObject gameObject;
    private IEnumerable<(FieldInfo field, SerializedProperty property)> fieldPropertyPairs;

    protected virtual void OnEnable()
    {
        var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var serializableFields = fields.Where(x => x.IsPublic || x.IsDefined(typeof(SerializeField), true));
        var serializedProperties = serializableFields.Select(x => serializedObject.FindProperty(x.Name));
        fieldPropertyPairs = serializableFields.Zip(serializedProperties, (x, y) => (x, y));
        gameObject = ((MonoBehaviour)target).gameObject;
    }

    private void HandleOnThisAttribute(VisualElement hierarchy, FieldInfo field, SerializedProperty property)
    {
        if (typeof(Component).IsAssignableFrom(field.FieldType))
        {
            // Check that field object is derived from Component, that's the only type that can be attached to GameObject.
            if (gameObject.TryGetComponent(field.FieldType, out var component))
            {
                property.objectReferenceValue = component;
            }
            else
            {
                var warningText = $@"This script requires component ""{field.FieldType.Name}"" to be present on this object.";
                hierarchy.Insert(0, new HelpBox(warningText, HelpBoxMessageType.Warning));
            }
        }
    }

    public override VisualElement CreateInspectorGUI()
    {
        var hierarchy = new VisualElement();

        foreach (var (field, property) in fieldPropertyPairs)
        {
            if (field.IsDefined(typeof(OnThisAttribute)))
                HandleOnThisAttribute(hierarchy, field, property);
            else
                hierarchy.Add(new PropertyField(property));
        }

        serializedObject.ApplyModifiedProperties();
        return hierarchy;
    }
}