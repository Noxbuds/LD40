using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for retrieving models and data about parts
// Data is retrieved via a function, list and stuff is manually
// edited for now...
[CreateAssetMenu(fileName = "Part Database", menuName = "Nox/Part Database", order = 1)]
public class PartDatabase : ScriptableObject {

    // Use public dictionaries to store the data
    public GameObject[] PrefabList;
    public ItemInfo[] ItemInfoList;

    // Item info struct
    public struct ItemInfo
    {
        public int ItemID;
        public string ItemName;
        public string ItemDescription;
    }
}
