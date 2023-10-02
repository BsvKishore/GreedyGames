/*using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/UIObjectHierarchy.json";
    private int selectedObjectIndex = -1;
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
            DisplayTemplateCreationSection();

            EditorGUILayout.Space();

            GUILayout.Label("Edit UI Object Hierarchy", EditorStyles.boldLabel);
            DisplayUIObjectList();
        }
    }

    private void DisplayTemplateCreationSection()
    {
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector2Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.FloatField("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector2Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

        if (GUILayout.Button("Create Template"))
        {
            CreateTemplate();
        }
    }

    private void DisplayUIObjectList()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < objectList.Count; i++)
        {
            var uiObject = objectList[i];

            if (GUILayout.Button("Edit Object " + i))
            {
                selectedObjectIndex = i;
                newTemplate = new UIObject()
                {
                    name = uiObject.name,
                    position = uiObject.position,
                    rotation = uiObject.rotation,
                    scale = uiObject.scale,
                    color = uiObject.color
                };
            }

            if (i == selectedObjectIndex)
            {
                uiObject.name = EditorGUILayout.TextField("Name", uiObject.name);
                uiObject.position = EditorGUILayout.Vector2Field("Position", uiObject.position);
                uiObject.rotation = EditorGUILayout.FloatField("Rotation", uiObject.rotation);
                uiObject.scale = EditorGUILayout.Vector2Field("Scale", uiObject.scale);
                uiObject.color = EditorGUILayout.ColorField("Color", uiObject.color);
            }

            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            hierarchy = JsonUtility.FromJson<UIObjectHierarchy>(jsonText);

            if (hierarchy != null && hierarchy.uiObjects != null)
            {
                objectList = new List<UIObject>(hierarchy.uiObjects);
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
            hierarchy.uiObjects = objectList.ToArray();

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
    public UIObject[] uiObjects;
}*/
/*
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/UIObjectHierarchy.json";
    private int selectedObjectIndex = -1;
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
            DisplayTemplateCreationSection();

            EditorGUILayout.Space();

            GUILayout.Label("Edit UI Object Hierarchy", EditorStyles.boldLabel);
            DisplayUIObjectList();
        }
    }

    private void DisplayTemplateCreationSection()
    {
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector2Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.FloatField("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector2Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

        if (GUILayout.Button("Create Template"))
        {
            CreateTemplate();
        }
    }

    private void DisplayUIObjectList()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < objectList.Count; i++)
        {
            var uiObject = objectList[i];

            if (GUILayout.Button("Edit Object " + i))
            {
                selectedObjectIndex = i;
                newTemplate = new UIObject()
                {
                    name = uiObject.name,
                    position = uiObject.position,
                    rotation = uiObject.rotation,
                    scale = uiObject.scale,
                    color = uiObject.color
                };
            }

            if (i == selectedObjectIndex)
            {
                uiObject.name = EditorGUILayout.TextField("Name", uiObject.name);
                uiObject.position = EditorGUILayout.Vector2Field("Position", uiObject.position);
                uiObject.rotation = EditorGUILayout.FloatField("Rotation", uiObject.rotation);
                uiObject.scale = EditorGUILayout.Vector2Field("Scale", uiObject.scale);
                uiObject.color = EditorGUILayout.ColorField("Color", uiObject.color);
            }

            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            hierarchy = JsonUtility.FromJson<UIObjectHierarchy>(jsonText);

            if (hierarchy != null && hierarchy.uiObjects != null)
            {
                objectList = new List<UIObject>(hierarchy.uiObjects);
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
            hierarchy.uiObjects = objectList.ToArray();

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
    public UIObject[] uiObjects;
}*/

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/UIObjectHierarchy.json";
    private int selectedObjectIndex = -1;
    private UIObjectHierarchy hierarchy;
    private List<UIObject> objectList = new List<UIObject>();
    private UIObject newTemplate = new UIObject();
    private Vector2 scrollPosition;
    private bool changesMade = false;

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
            DisplayTemplateCreationSection();

            EditorGUILayout.Space();

            GUILayout.Label("Edit UI Object Hierarchy", EditorStyles.boldLabel);
            DisplayUIObjectList();

            // Apply button to apply changes
            if (selectedObjectIndex >= 0 && selectedObjectIndex < objectList.Count)
            {
                if (changesMade)
                {
                    if (GUILayout.Button("Apply Changes"))
                    {
                        ApplyChanges();
                    }
                }
            }
        }
    }

    private void DisplayTemplateCreationSection()
    {
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector2Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.FloatField("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector2Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);

        // RawImage properties
        newTemplate.texture = (Texture)EditorGUILayout.ObjectField("RawImage Texture", newTemplate.texture, typeof(Texture), false);
        newTemplate.rawImageEnabled = EditorGUILayout.Toggle("RawImage Enabled", newTemplate.rawImageEnabled);

        // Text properties
        newTemplate.text = EditorGUILayout.TextField("Text", newTemplate.text);
        newTemplate.textColor = EditorGUILayout.ColorField("Text Color", newTemplate.textColor);
        newTemplate.textFont = (Font)EditorGUILayout.ObjectField("Text Font", newTemplate.textFont, typeof(Font), false);

        // Image properties
        newTemplate.sprite = (Sprite)EditorGUILayout.ObjectField("Image Sprite", newTemplate.sprite, typeof(Sprite), false);
        newTemplate.imageEnabled = EditorGUILayout.Toggle("Image Enabled", newTemplate.imageEnabled);

        if (GUILayout.Button("Create Template"))
        {
            CreateTemplate();
        }
    }

    private void DisplayUIObjectList()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < objectList.Count; i++)
        {
            var uiObject = objectList[i];

            if (GUILayout.Button("Edit Object " + i))
            {
                selectedObjectIndex = i;
                newTemplate = new UIObject()
                {
                    name = uiObject.name,
                    position = uiObject.position,
                    rotation = uiObject.rotation,
                    scale = uiObject.scale,
                    color = uiObject.color,
                    // RawImage properties
                    texture = uiObject.texture,
                    rawImageEnabled = uiObject.rawImageEnabled,
                    // Text properties
                    text = uiObject.text,
                    textColor = uiObject.textColor,
                    textFont = uiObject.textFont,
                    // Image properties
                    sprite = uiObject.sprite,
                    imageEnabled = uiObject.imageEnabled
                };

                // Reset the changesMade flag when a new object is selected
                changesMade = false;
            }

            if (i == selectedObjectIndex)
            {
                // Display object properties and track changes
                uiObject.name = EditorGUILayout.TextField("Name", uiObject.name);
                uiObject.position = EditorGUILayout.Vector2Field("Position", uiObject.position);
                uiObject.rotation = EditorGUILayout.FloatField("Rotation", uiObject.rotation);
                uiObject.scale = EditorGUILayout.Vector2Field("Scale", uiObject.scale);
                uiObject.color = EditorGUILayout.ColorField("Color", uiObject.color);

                // RawImage properties
                uiObject.texture = (Texture)EditorGUILayout.ObjectField("RawImage Texture", uiObject.texture, typeof(Texture), false);
                uiObject.rawImageEnabled = EditorGUILayout.Toggle("RawImage Enabled", uiObject.rawImageEnabled);

                // Text properties
                uiObject.text = EditorGUILayout.TextField("Text", uiObject.text);
                uiObject.textColor = EditorGUILayout.ColorField("Text Color", uiObject.textColor);
                uiObject.textFont = (Font)EditorGUILayout.ObjectField("Text Font", uiObject.textFont, typeof(Font), false);

                // Image properties
                uiObject.sprite = (Sprite)EditorGUILayout.ObjectField("Image Sprite", uiObject.sprite, typeof(Sprite), false);
                uiObject.imageEnabled = EditorGUILayout.Toggle("Image Enabled", uiObject.imageEnabled);

                // Check if changes have been made
                CheckForChanges(uiObject);
            }

            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }

    private void CheckForChanges(UIObject uiObject)
    {
        // Compare the properties of the UIObject with the newTemplate
        if (uiObject.name != newTemplate.name ||
            uiObject.position != newTemplate.position ||
            uiObject.rotation != newTemplate.rotation ||
            uiObject.scale != newTemplate.scale ||
            uiObject.color != newTemplate.color ||
            uiObject.texture != newTemplate.texture ||
            uiObject.rawImageEnabled != newTemplate.rawImageEnabled ||
            uiObject.text != newTemplate.text ||
            uiObject.textColor != newTemplate.textColor ||
            uiObject.textFont != newTemplate.textFont ||
            uiObject.sprite != newTemplate.sprite ||
            uiObject.imageEnabled != newTemplate.imageEnabled)
        {
            changesMade = true;
        }
        else
        {
            changesMade = false;
        }
    }

    private void ApplyChanges()
    {
        if (selectedObjectIndex >= 0 && selectedObjectIndex < objectList.Count)
        {
            objectList[selectedObjectIndex] = new UIObject()
            {
                name = newTemplate.name,
                position = newTemplate.position,
                rotation = newTemplate.rotation,
                scale = newTemplate.scale,
                color = newTemplate.color,
                texture = newTemplate.texture,
                rawImageEnabled = newTemplate.rawImageEnabled,
                text = newTemplate.text,
                textColor = newTemplate.textColor,
                textFont = newTemplate.textFont,
                sprite = newTemplate.sprite,
                imageEnabled = newTemplate.imageEnabled
            };

            // Reset changesMade flag
            changesMade = false;
        }
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            hierarchy = JsonUtility.FromJson<UIObjectHierarchy>(jsonText);

            if (hierarchy != null && hierarchy.uiObjects != null)
            {
                objectList = new List<UIObject>(hierarchy.uiObjects);
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
            hierarchy.uiObjects = objectList.ToArray();

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

    // Properties for RawImage component
    public Texture texture;
    public bool rawImageEnabled;

    // Properties for Text component
    public string text;
    public Color textColor;
    public Font textFont;

    // Properties for Image component
    public Sprite sprite;
    public bool imageEnabled;
}

[System.Serializable]
public class UIObjectHierarchy
{
    public UIObject[] uiObjects;
}