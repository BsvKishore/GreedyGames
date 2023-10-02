using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomEditorJson : EditorWindow
{
    private string jsonFilePath = "Assets/ObjectHierarchy.json";
    private int selectedObjectIndex = -1;
    private UIObjectHierarchy hierarchy;
    private List<UIObject> objectList = new List<UIObject>();
    private UIObject newTemplate = new UIObject();
    private Vector2 scrollPosition;
    private bool changesMade = false;

    private GameObject canvas;

    [MenuItem("Window/Custom Editor Json")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorJson>("Custom Editor Json");

    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Editor Json", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        GUILayout.Label("Current Editor/Selected Editor", EditorStyles.boldLabel);
        DisplayTemplateCreationSection();
        EditorGUILayout.Space();

        if (GUILayout.Button("Load JSON"))
        {
            LoadJSON();
        }
        if (GUILayout.Button("Save JSON"))
        {
            SaveJSON();
        }
        if (GUILayout.Button("Delete JSON"))
        {
            DeleteJSON();
        }
        if (GUILayout.Button("Create UI"))
        {
            InstantiateUIObjects();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Created Templates", EditorStyles.boldLabel);
        if (objectList != null && hierarchy != null)
        {
            
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
            // Delete the game object from the Unity hierarchy
            DeleteGameObjectFromHierarchy(objectList[index].name);

            objectList.RemoveAt(index);
            changesMade = false;

            if (hierarchy != null && hierarchy.uiObjects != null)
            {
                List<UIObject> hierarchyList = new List<UIObject>(hierarchy.uiObjects);
                hierarchyList.RemoveAt(index);
                hierarchy.uiObjects = hierarchyList.ToArray();
            }
        }
    }

    private void DeleteGameObjectFromHierarchy(string gameObjectName)
    {
        GameObject gameObjectToDelete = GameObject.Find(gameObjectName);
        if (gameObjectToDelete != null)
        {
            DestroyImmediate(gameObjectToDelete);
        }
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < objectList.Count;
    }

    private void DisplayTemplateCreationSection()
    {
        DisplayUIObjectFields(newTemplate);
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
                DisplayUIObjectFields(uiObject);
                CheckForChanges(uiObject);
            }

            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DisplayUIObjectFields(UIObject uiObject)
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
    }

    private void CheckForChanges(UIObject uiObject)
    {
        changesMade = !uiObject.IsSameAs(newTemplate);
    }

    private void ApplyChanges()
    {
        if (IsValidIndex(selectedObjectIndex))
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
            CreateCanvas();
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
                rawImage.color = uiObject.color;
            }

            if (!string.IsNullOrEmpty(uiObject.text) && !uiObject.rawImageEnabled)
            {
                Text text = uiObjectPrefab.AddComponent<Text>();
                text.text = uiObject.text;
                text.color = uiObject.textColor;
                text.font = uiObject.textFont;
            }
            else
            {
                Debug.LogWarning("Image Can't Added To Rawimage");
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

    private void CreateCanvas()
    {
        canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
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

    public bool IsSameAs(UIObject other)
    {
        return name == other.name && position == other.position && rotation == other.rotation && scale == other.scale && color == other.color && texture == other.texture && rawImageEnabled == other.rawImageEnabled && text == other.text && textColor == other.textColor && textFont == other.textFont;
    }
}

[System.Serializable]
public class UIObjectHierarchy
{
    public UIObject[] uiObjects;
}