using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartDatabase))]
public class PartDatabaseEditor : Editor {

    int numberOfElements;

    public override void OnInspectorGUI()
    {
        // Just use default UI for the prefabs
        //base.OnInspectorGUI();

        // Use custom UI for item info (since Unity's doesn't work for structs)
        PartDatabase ptarget = (PartDatabase)target;

        // Initialise the lists if they haven't been
        if (ptarget.PrefabList == null) ptarget.PrefabList = new GameObject[0];
        if (ptarget.ItemInfoList == null) ptarget.ItemInfoList = new PartDatabase.ItemInfo[0];

        // Item Info array resizing
        EditorGUILayout.LabelField("Item Info:");
        numberOfElements = ptarget.ItemInfoList.Length;
        numberOfElements = EditorGUILayout.IntField("Count", numberOfElements);

        if (numberOfElements != ptarget.ItemInfoList.Length)
        {
            // Resize the arrays then
            Array.Resize<GameObject>(ref ptarget.PrefabList, numberOfElements);
            Array.Resize<PartDatabase.ItemInfo>(ref ptarget.ItemInfoList, numberOfElements);
        }

        // Iterate through each element in the item info dictionary
        for (int i = 0; i < ptarget.ItemInfoList.Length; i++)
        {
            // Very basic UI for the UI
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item ID: " + i);
            ptarget.ItemInfoList[i].ItemName = EditorGUILayout.TextField("Item Name", ptarget.ItemInfoList[i].ItemName);
            ptarget.ItemInfoList[i].ItemDescription = EditorGUILayout.TextField("Item Description", ptarget.ItemInfoList[i].ItemDescription);
            ptarget.PrefabList[i] = (GameObject)EditorGUILayout.ObjectField("Item Prefab", ptarget.PrefabList[i], typeof(GameObject));
        }
    }
}
