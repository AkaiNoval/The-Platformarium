using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GameAssets))]
public class GameAssetsEnumDrawerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var enumDrawer = (GameAssets)target;
        if (enumDrawer == null) return;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Drawers");
        
    }

}
#endif