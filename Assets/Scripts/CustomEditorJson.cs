using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/UIObjectHierarchy.json";
    private int selectedObjectIndex = -1;
    private UIObjectHierarchy hierarchy;
    private List<UIObject> objectList = new List<UIObject>();
    private UIObject newTemplate = new UIObject();
    private Vector2 scrollPosition;
    private bool changesMade = false;

    private GameObject canvas; // Reference to the Canvas where templates will be instantiated

    [MenuItem("Window/Custom Editor Json")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorJson>("Custom Editor Json");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Editor Json", EditorStyles.boldLabel);

        if (GUILayout.Button("Load JSON")) LoadJSON();
        if (GUILayout.Button("Save JSON")) SaveJSON();
        if (GUILayout.Button("Delete JSON")) DeleteJSON();
        if (GUILayout.Button("Instantiate")) InstantiateUIObjects();

        EditorGUILayout.Space();

        if (objectList != null && hierarchy != null)
        {
            DisplayTemplateCreationSection();
            EditorGUILayout.Space();
            DisplayUIObjectList();

            if (selectedObjectIndex >= 0 && selectedObjectIndex < objectList.Count)
            {
                if (changesMade && GUILayout.Button("Apply Changes")) ApplyChanges();
                if (GUILayout.Button("Delete Template")) DeleteTemplate(selectedObjectIndex);
            }
        }
    }

    private void DeleteTemplate(int index)
    {
        if (index >= 0 && index < objectList.Count)
        {
            objectList.RemoveAt(index);
            changesMade = false;
        }
    }

    private void DisplayTemplateCreationSection()
    {
        newTemplate.name = EditorGUILayout.TextField("Name", newTemplate.name);
        newTemplate.position = EditorGUILayout.Vector2Field("Position", newTemplate.position);
        newTemplate.rotation = EditorGUILayout.FloatField("Rotation", newTemplate.rotation);
        newTemplate.scale = EditorGUILayout.Vector2Field("Scale", newTemplate.scale);
        newTemplate.color = EditorGUILayout.ColorField("Color", newTemplate.color);
        newTemplate.texture = (Texture)EditorGUILayout.ObjectField("RawImage Texture", newTemplate.texture, typeof(Texture), false);
        newTemplate.rawImageEnabled = EditorGUILayout.Toggle("RawImage Enabled", newTemplate.rawImageEnabled);
        newTemplate.text = EditorGUILayout.TextField("Text", newTemplate.text);
        newTemplate.textColor = EditorGUILayout.ColorField("Text Color", newTemplate.textColor);
        newTemplate.textFont = (Font)EditorGUILayout.ObjectField("Text Font", newTemplate.textFont, typeof(Font), false);

        if (GUILayout.Button("Create Template")) CreateTemplate();
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
                newTemplate = new UIObject(uiObject);
                changesMade = false;
            }

            if (i == selectedObjectIndex)
            {
                uiObject.name = EditorGUILayout.TextField("Name", uiObject.name);
                uiObject.position = EditorGUILayout.Vector2Field("Position", uiObject.position);
                uiObject.rotation = EditorGUILayout.FloatField("Rotation", uiObject.rotation);
                uiObject.scale = EditorGUILayout.Vector2Field("Scale", uiObject.scale);
                uiObject.color = EditorGUILayout.ColorField("Color", uiObject.color);
                uiObject.texture = (Texture)EditorGUILayout.ObjectField("RawImage Texture", uiObject.texture, typeof(Texture), false);
                uiObject.rawImageEnabled = EditorGUILayout.Toggle("RawImage Enabled", uiObject.rawImageEnabled);
                uiObject.text = EditorGUILayout.TextField("Text", uiObject.text);
                uiObject.textColor = EditorGUILayout.ColorField("Text Color", uiObject.textColor);
                uiObject.textFont = (Font)EditorGUILayout.ObjectField("Text Font", uiObject.textFont, typeof(Font), false);
                CheckForChanges(uiObject);
            }

            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }

    private void CheckForChanges(UIObject uiObject)
    {
        if (uiObject.IsDifferentFrom(newTemplate))
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
            objectList[selectedObjectIndex] = new UIObject(newTemplate);
            changesMade = false;
        }
    }

    private void LoadJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            hierarchy = JsonUtility.FromJson<UIObjectHierarchy>(jsonText);

            if (hierarchy?.uiObjects != null)
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

    private void DeleteJSON()
    {
        if (File.Exists(jsonFilePath))
        {
            File.Delete(jsonFilePath);
            Debug.Log("Deleted JSON file at " + jsonFilePath);
            objectList.Clear();
            hierarchy = null;
        }
        else
        {
            Debug.LogWarning("JSON file not found for deletion.");
        }
    }

    private void InstantiateUIObjects()
    {
        if (canvas == null)
        {
            // If there is no canvas, create one
            canvas = new GameObject("Canvas");
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            // Optionally, create an EventSystem if it doesn't exist
            if (FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        foreach (UIObject uiObject in objectList)
        {
            GameObject uiObjectPrefab = new GameObject(uiObject.name);
            uiObjectPrefab.transform.SetParent(canvas.transform);
            uiObjectPrefab.transform.localPosition = uiObject.position;
            uiObjectPrefab.transform.localRotation = Quaternion.Euler(0, 0, uiObject.rotation);
            uiObjectPrefab.transform.localScale = uiObject.scale;

            if (uiObject.rawImageEnabled)
            {
                RawImage rawImage = uiObjectPrefab.AddComponent<RawImage>();
                rawImage.texture = uiObject.texture;
            }

            if (!string.IsNullOrEmpty(uiObject.text))
            {
                Text text = uiObjectPrefab.AddComponent<Text>();
                text.text = uiObject.text;
                text.color = uiObject.textColor;
                text.font = uiObject.textFont;
            }
        }
    }

    private void CreateTemplate()
    {
        if (objectList != null)
        {
            objectList.Add(new UIObject(newTemplate));
            newTemplate = new UIObject();
        }
        else
        {
            objectList = new List<UIObject>();
            objectList.Add(new UIObject(newTemplate));
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
    public Texture texture;
    public bool rawImageEnabled;
    public string text;
    public Color textColor;
    public Font textFont;

    public UIObject() { }

    public UIObject(UIObject other)
    {
        name = other.name;
        position = other.position;
        rotation = other.rotation;
        scale = other.scale;
        color = other.color;
        texture = other.texture;
        rawImageEnabled = other.rawImageEnabled;
        text = other.text;
        textColor = other.textColor;
        textFont = other.textFont;
    }

    public bool IsDifferentFrom(UIObject other)
    {
        return name != other.name ||
            position != other.position ||
            rotation != other.rotation ||
            scale != other.scale ||
            color != other.color ||
            texture != other.texture ||
            rawImageEnabled != other.rawImageEnabled ||
            text != other.text ||
            textColor != other.textColor ||
            textFont != other.textFont;
    }
}

[System.Serializable]
public class UIObjectHierarchy
{
    public UIObject[] uiObjects;
}