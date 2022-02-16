using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Staggart Creations http://staggart.xyz
// Copyright protected under Unity asset store EULA

public class ReplaceSelected : EditorWindow
{
#if UNITY_2019_3_OR_NEWER
    private const float HEIGHT = 235f;
#else
    private const float HEIGHT = 220f;
#endif

    [MenuItem("GameObject/Replace selected")]
    public static void OpenWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        ReplaceSelected window = (ReplaceSelected)EditorWindow.GetWindow(typeof(ReplaceSelected), true);

        //Options
        window.autoRepaintOnSceneChange = true;
        window.maxSize = new Vector2(230f, HEIGHT);
        window.minSize = window.maxSize;
        window.titleContent.image = EditorGUIUtility.IconContent("GameObject Icon").image;
        window.titleContent.text = "Replace selected";

        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    [SerializeField]
    private Object sourceObject;
    private static Object sourcePrefab;
    
    private void OnEnable()
    {
        if (sourceObject != null) return;

        if (LastTargetGUID != string.Empty)
        {
            string path = AssetDatabase.GUIDToAssetPath(LastTargetGUID);
            sourceObject = (Object)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        }
    }

    private static string LastTargetGUID
    {
        get { return EditorPrefs.GetString(PlayerSettings.productName + "_REPLACE_LASTGUID", string.Empty); }
        set { EditorPrefs.SetString(PlayerSettings.productName + "_REPLACE_LASTGUID", value); }
    }

    private static bool KeepScale
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepScale", true); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepScale", value); }
    }
    private static bool KeepRotation
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepRotation", true); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepRotation", value); }
    }
    private static bool KeepPrefabOverrides
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepPrefabOverrides", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepPrefabOverrides", value); }
    }
    private static bool KeepLayer
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepLayer", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepLayer", value); }
    }
    private static bool KeepTag
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepTag", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepTag", value); }
    }
    private static bool KeepStaticFlags
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepStaticFlags", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepStaticFlags", value); }
    }

    private void OnGUI()
    {
        if (Selection.gameObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("Nothing selected", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField("Replacement object/prefab", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        sourceObject = (Object)EditorGUILayout.ObjectField(sourceObject, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck())
        {
            LastTargetGUID = sourceObject ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sourceObject.GetInstanceID())) : string.Empty;
        }
        EditorGUILayout.Space();

        using (new EditorGUI.DisabledGroupScope(sourceObject == null))
        {
            EditorGUILayout.LabelField("Keep object's", EditorStyles.boldLabel);
            KeepScale = EditorGUILayout.Toggle(new GUIContent("Scale", "Enable to keep the current object's scale"), KeepScale);
            KeepRotation = EditorGUILayout.Toggle(new GUIContent("Rotation", "Enable to keep the current object's rotation"), KeepRotation);
            KeepLayer = EditorGUILayout.Toggle(new GUIContent("Layer", "Enable to keep the current object's layer"), KeepLayer);
            KeepTag = EditorGUILayout.Toggle(new GUIContent("Tag", "Enable to keep the current object's tag"), KeepTag);
            KeepStaticFlags = EditorGUILayout.Toggle(new GUIContent("Static flags", "Enable to keep the current object's static flags"), KeepStaticFlags);
            KeepPrefabOverrides = EditorGUILayout.Toggle(new GUIContent("Prefab overrides", "If the selected object is a prefab with overrides, these are copied over to the replaced object"), KeepPrefabOverrides);

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(" Replace " + Selection.gameObjects.Length + " GameObject" + (Selection.gameObjects.Length > 1 ? "s" : ""), EditorGUIUtility.IconContent("Refresh").image), GUILayout.Height(25f)))
            {
                ReplaceCurrentSelection();
            }
        }
    }

    private void OnSelectionChange()
    {
        this.Repaint();
    }

    private void ReplaceCurrentSelection()
    {
        if (Selection.gameObjects.Length == 0 || sourceObject == null) return;

        sourcePrefab = null;
        //Note: No way to get the prefab variant, will always get the base
        sourcePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(sourceObject);

        //Model prefabs don't count
        isPrefab = sourcePrefab && (PrefabUtility.GetPrefabAssetType(sourceObject) != PrefabAssetType.Model);
        
        foreach (GameObject selected in Selection.gameObjects)
        {
            Replace(sourceObject, selected);
        }
    }

    private static bool isPrefab = false;

    private static void Replace(Object source, GameObject destination)
    {
        //Skip anything selected in the project window!
        if (destination.scene.IsValid() == false) return;
        
        GameObject newObj = null;

        if (PrefabUtility.IsPartOfPrefabInstance(destination) && !PrefabUtility.IsOutermostPrefabInstanceRoot(destination))
        {
            Debug.LogError("Cannot replace an object that's part of a prefab instance", destination);
            return;
        }

        if (isPrefab)
        {
            newObj = PrefabUtility.InstantiatePrefab(sourcePrefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(newObj, "Replaced with prefabs");

            //Apply any overrides (added/removed components, parameters, etc)
            if (PrefabUtility.HasPrefabInstanceAnyOverrides(destination, false) && KeepPrefabOverrides)
            {
                //Get all overrides
                PropertyModification[] overrides = PrefabUtility.GetPropertyModifications(destination);
                List<AddedComponent> added = PrefabUtility.GetAddedComponents(destination);
                //List<ObjectOverride> objOverrides = PrefabUtility.GetObjectOverrides(source);
                List<RemovedComponent> removed = PrefabUtility.GetRemovedComponents(destination);

                //Remove any components removed as an overrides
                for (int i = 0; i < removed.Count; i++)
                {
                    Component comp = newObj.GetComponent(removed[i].assetComponent.GetType());
                    DestroyImmediate(comp);
                }

                //Add any components added as overrides and copy the values over
                for (int i = 0; i < added.Count; i++)
                {
                    Component copy = newObj.AddComponent(added[i].instanceComponent.GetType());
                    EditorUtility.CopySerialized(added[i].instanceComponent, copy);
                }

                //PrefabUtility.ApplyPrefabInstance(target, InteractionMode.AutomatedAction);

                //Apply any modified parameters
                PrefabUtility.SetPropertyModifications(newObj, overrides);
            }
        }
        else
        {
            newObj = GameObject.Instantiate(source) as GameObject;
            Undo.RegisterCreatedObjectUndo(newObj, "Replaced object");
            newObj.name = newObj.name.Replace("(Clone)", string.Empty);
        }

        newObj.transform.parent = destination.transform.parent;
        newObj.transform.SetSiblingIndex(destination.transform.GetSiblingIndex());
        newObj.transform.position = destination.transform.position;

        if (KeepRotation) newObj.transform.rotation = destination.transform.rotation;
        if (KeepScale) newObj.transform.localScale = destination.transform.localScale;
        if (KeepTag) newObj.tag = destination.tag;
        if (KeepLayer) newObj.layer = destination.layer;
        if (KeepStaticFlags) GameObjectUtility.SetStaticEditorFlags(newObj, GameObjectUtility.GetStaticEditorFlags(destination));

        if (Selection.gameObjects.Length == 1) Selection.activeGameObject = newObj;
        
        EditorSceneManager.MarkSceneDirty(destination.scene);
        
        //Remove the original object (can cause a crash if it is a prefab due to a Unity bug (2019.3))
        Undo.DestroyObjectImmediate(destination);
    }
}