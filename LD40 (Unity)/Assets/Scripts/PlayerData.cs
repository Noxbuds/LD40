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
    public int LevelNumber;
    public int MaxLevels;

    // Amount of gold the player has (moved into player data)
    // You can't have e.g 1.5 gold (well I guess you can but...)
    // so it can do with being an int - will be more accurate

	// Use this for initialization
	void Awake ()
    {
        // Set save file path
        SaveFilePath = Application.dataPath + "/player.dat";

        // Load the player's save
        LoadProgress();
	}

    // Save the player data with a simple data
    // class that's serialisable
    public void SaveProgress()
    {
        Debug.Log("Saving progress.");

        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);

        using (Stream stream = File.Open(SaveFilePath, FileMode.CreateNew))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, playerData);

            // Update music volume
            Camera.main.GetComponent<AudioSource>().volume = GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume;
        }
    }

    // Loads the player's progress from the file
    public void LoadProgress()
    {
        // Create a new save file
        if (!File.Exists(SaveFilePath))
        {
            Debug.Log("Creating new file.");

            playerData = new SPData();
            playerData.Gold = 0;
            playerData.LevelsWon = new bool[MaxLevels];
            playerData.MusicVolume = 1;

            // Create an empty parts list; if a slot is -1 it means
            // there is no part there
            playerData.Parts = new int[Basher.MaxParts];
            for (int i = 0; i < playerData.Parts.Length; i++)
                playerData.Parts[i] = -1;

            // Create a list of owned parts
            playerData.PartsOwned = new bool[GameObject.FindObjectOfType<PartDatabase>().ItemInfoList.Length];
            for (int i = 0; i < playerData.PartsOwned.Length; i++)
                playerData.PartsOwned[i] = false;

            // Create a list of tutorials completed
            playerData.TutorialsViewed = new bool[DialogueManager.TutorialCount];
            for (int i = 0; i < playerData.TutorialsViewed.Length; i++)
                playerData.TutorialsViewed[i] = false;

            // Create an empty enchantments list... same as above
            playerData.Enchantments = new int[Basher.MaxEnchantments];
            for (int i = 0; i < playerData.Enchantments.Length; i++)
                playerData.Enchantments[i] = -1;

            // Save into file
            SaveProgress();
        }
        else
        {
            Debug.Log("Loading from file.");
            // If the file has just been created, it means that our playerData
            // variable already contains data - no need to load it again.
            using (Stream stream = File.Open(SaveFilePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SPData newData = (SPData)formatter.Deserialize(stream);

                // Update the local variable of the player data
                playerData = newData;

                // Update music volume
                Camera.main.GetComponent<AudioSource>().volume = GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume;
            }
        }
    }

    // Does all necessary end-of-game things
    public void GameEnded(bool gameWon)
    {
        // Verify that the levels won array exists
        if (playerData.LevelsWon == null || playerData.LevelsWon.Length == 0)
            playerData.LevelsWon = new bool[MaxLevels];

        // Update player's win status in the player data
        if (gameWon)
            playerData.LevelsWon[LevelNumber] = true;

        SaveProgress();
    }

    // Returns gold amount...
    public int GetGoldAmount()
    {
        return playerData.Gold;
    }

    // Award some gold
    public void AwardGold(int amount)
    {
        playerData.Gold += amount;
    }

    // Serialisable player data class
    [Serializable]
    public class SPData
    {
        [SerializeField]
        public int Gold;
        [SerializeField]
        public bool[] LevelsWon;
        [SerializeField]
        public int[] Parts;
        [SerializeField]
        public int[] Enchantments;
        [SerializeField]
        public bool[] PartsOwned;
        [SerializeField]
        public float MusicVolume;
        [SerializeField]
        public bool[] TutorialsViewed; // 1 element per tutorial...
    }
}
