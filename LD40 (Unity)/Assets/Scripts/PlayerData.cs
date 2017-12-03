using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    // Basic options and stuff
    public string SaveFilePath;
    public SPData playerData;

    // Amount of gold the player has
    // You can't have e.g 1.5 gold (well I guess you can but...)
    // so it can do with being an int - will be more accurate
    public int Gold;

	// Use this for initialization
	void Start ()
    {
        // Set save file path
        SaveFilePath = Application.dataPath + "/Save/player.data";
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Save the player data with a simple data
    // class that's serialisable
    void SaveProgress()
    {
        using (Stream stream = File.Open(SaveFilePath, FileMode.CreateNew))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, playerData);
        }
    }

    public int GetGoldAmount()
    {
        return Gold;
    }

    // Award some gold
    public void AwardGold(int amount)
    {
        Gold += amount;
    }

    // Serialisable player data class
    [Serializable]
    public class SPData
    {
        [SerializeField]
        public string Username;
        [SerializeField]
        public int Gold;
    }
}
