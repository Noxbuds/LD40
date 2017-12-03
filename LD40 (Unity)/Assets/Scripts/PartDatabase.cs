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

    /// <summary>
    /// Get the prefab associated with a specified item ID
    /// </summary>
    /// <param name="id">The ID of the item prefab you're trying to get</param>
    /// <returns></returns>
    public GameObject RetrievePrefab(int id)
    {
        if (id < PrefabList.Length)
            return PrefabList[id];
        else
            return null;
    }

    /// <summary>
    /// Get the item info (name, description) of an item
    /// </summary>
    /// <param name="id">The ID of the item you're looking for</param>
    /// <returns></returns>
    public ItemInfo RetrieveData(int id)
    {
        if (id < ItemInfoList.Length)
            return ItemInfoList[id];
        else
            return null;
    }
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
    [SerializeField]
    public bool IsEnchantment;
    [SerializeField]
    public int GoldCost;
}