using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/UIObjectHierarchy.json";

    private UIObjectHierarchy hierarchy;
    private List<UIObject> objectList = new List<UIObject>();
    private UIObject newTemplate = new UIObject();
    private Vector2 scrollPosition;

    [MenuItem("Window/Custom Editor Json")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorJson>("Custom Editor Json");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Editor Json", EditorStyles.boldLabel);

        if (GUILayout.Button("Load JSON"))
        {
            LoadJSON();
        }

        if (GUILayout.Button("Save JSON"))
        {
            SaveJSON();
        }

        EditorGUILayout.Space();

        if (objectList != null && hierarchy != null)
        {
            GUILayout.Label("Create New Template", EditorStyles.boldLabel);
            newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
            newTemplate.position = EditorGUILayout.Vector2Field("Position", newTemplate.position);
            newTemplate.rotation = EditorGUILayout.FloatField("Rotation", newTemplate.rotation);
            newTemplate.scale = EditorGUILayout.Vector2Field("Scale", newTemplate.scale);
            newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

            if (GUILayout.Button("Create Template"))
            {
                CreateTemplate();
            }

            EditorGUILayout.Space();

            GUILayout.Label("Edit UI Object Hierarchy", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var uiObject in objectList)
            {
                EditorGUILayout.LabelField("Name: " + uiObject.name);
                EditorGUILayout.Vector2Field("Position", uiObject.position);
                uiObject.rotation = EditorGUILayout.FloatField("Rotation", uiObject.rotation);
                EditorGUILayout.Vector2Field("Scale", uiObject.scale);
                uiObject.color = EditorGUILayout.ColorField("Color", uiObject.color);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            hierarchy = JsonUtility.FromJson<UIObjectHierarchy>(jsonText);

            if (hierarchy != null && hierarchy.objects != null)
            {
                objectList = new List<UIObject>(hierarchy.objects);
            }
            else
            {
                Debug.LogWarning("JSON file is empty.");
                objectList.Clear();
            }
        }
        else
        {
            Debug.LogWarning("JSON file not found.");
            objectList.Clear();
        }
    }

    private void SaveJSON()
    {
        if (objectList != null)
        {
            hierarchy.objects = objectList.ToArray();

            string jsonText = JsonUtility.ToJson(hierarchy);
            File.WriteAllText(jsonFilePath, jsonText);

            Debug.Log("JSON file saved at " + jsonFilePath);
        }
        else
        {
            Debug.LogWarning("Nothing to save. UI Object Hierarchy is empty.");
        }
    }

    private void CreateTemplate()
    {
        if (objectList != null)
        {
            objectList.Add(newTemplate);
            newTemplate = new UIObject();
        }
        else
        {
            objectList = new List<UIObject>();
            objectList.Add(newTemplate);
            newTemplate = new UIObject();
        }
    }
}

[System.Serializable]
public class UIObject
{
    public string name;
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Color color;
}

[System.Serializable]
public class UIObjectHierarchy
{
    public UIObject[] objects;
}