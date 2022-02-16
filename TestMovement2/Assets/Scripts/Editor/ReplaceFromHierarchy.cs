using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]

public class ReplaceFromHierarchy : MonoBehaviour
{
    [MenuItem("GameObject/Replace", false, 0)]
    private static void OpenWindow()
    {
        ReplaceSelected.OpenWindow();
    }
}
