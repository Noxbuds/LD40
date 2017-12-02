using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for retrieving models and data about parts
// Data is retrieved via a function, list and stuff is manually
// edited for now...
public class PartDatabase : MonoBehaviour {

    // Use public dictionaries to store the data
    public GameObject[] PrefabList;
    public ItemInfo[] ItemInfoList;
}

// Item info struct
[Serializable]
public class ItemInfo
{
    [SerializeField]
    public int ItemID;
    [SerializeField]
    public string ItemName;
    [SerializeField]
    public string ItemDescription;
}