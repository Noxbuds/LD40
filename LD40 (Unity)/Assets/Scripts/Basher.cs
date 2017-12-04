using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
public class Basher : MonoBehaviour
{
    // The main script that controls the battering ram

    // Local references
    private Basher _this;
    public BasherPart[] parts;
    public BasherPart[] enchantments;
    public float[] ResistMults;

    public float SpeedMultiplier;
    public static int MaxParts = 2;
    public static int MaxEnchantments = 1;

    public bool GameWon;
    public bool GameEnded;

	// Use this for initialization
	void Start ()
    {
        Application.targetFrameRate = 0;

        _this = this.gameObject.GetComponent<Basher>();

        this.GetComponent<Damagable>().IsEnemy = false;

        if (parts == null) parts = new BasherPart[MaxParts];
        if (enchantments == null) enchantments = new BasherPart[MaxEnchantments];
        ResistMults = new float[MaxParts + MaxEnchantments];

        // Set default Damagable values for the basher
        Damagable dmgb = this.GetComponent<Damagable>();
        dmgb.Health = 100;
        dmgb.Resistance = 1;
        SpeedMultiplier = 1;

        // Fetch a copy of the player data to shorten the code a bit
        // and reduce CPU strain (slightly)
        PlayerData.SPData playerData = GameObject.FindObjectOfType<PlayerData>().playerData;

        // Add each part to the parts list with the help of the part database
        PartDatabase partDB = GameObject.FindObjectOfType<PartDatabase>();
        
        // Create the parts
        for (int i = 0; i < MaxParts; i++)
        {
            parts[i] = CreatePart(playerData.Parts[i]);
        }

        // Create the enchantments
        for (int i = 0; i < MaxEnchantments; i++)
        {
            enchantments[i] = CreatePart(playerData.Enchantments[i]);
        }

        // Go through parts and call the Init function
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
            {
                parts[i].Initialise(ref _this);
                parts[i].SpawnPrefab();
            }

            // Just set to 1 so we don't automatically become invincible
            ResistMults[i] = 1;
        }

        // Go through enchantments and call the Init function
        for (int i = 0; i < enchantments.Length; i++)
        {
            if (enchantments[i] != null)
            {
                enchantments[i].Initialise(ref _this);
                enchantments[i].SpawnPrefab();
            }

            // Just set to 1 so we don't automatically become invincible
            ResistMults[MaxParts + i] = 1;
        }
	}
	
    BasherPart CreatePart(int id)
    {
        BasherPart part = null;

        // No default case needed since it's already assigned to null
        switch (id)
        {
            case 0:
                part = new PartHeavyPlating();
                break;
            case 1:
                part = new PartHeavyShield();
                break;
            case 2:
                part = new EnchantHealth();
                break;
            case 3:
                part = new EnchantSacrifice();
                break;
            case 4:
                part = new EnchantSpeed();
                break;
            case 5:
                part = new PartWindmill();
                break;
            case 6:
                part = new PartSail();
                break;
        }

        // Return it
        if (part != null)
            return part;
        else
            return null;
    }

	// Update is called once per frame
	void Update ()
    {
        this.GetComponent<Damagable>().Resistance = 0.9f;

		// Trigger each part's update
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
                parts[i].Update(ref _this);
        }

        // Make a 'running total' of the resistance
        for (int i = 0; i < MaxParts + MaxEnchantments; i++)
            this.GetComponent<Damagable>().Resistance *= ResistMults[i];

        // Trigger each enchantment's update
        for (int i = 0; i < enchantments.Length; i++)
        {
            if (enchantments[i] != null)
                enchantments[i].Update(ref _this);
        }

        // Check if the player has died
        if (this.GetComponent<Damagable>().Health <= 0)
            GameLose();
	}

    // Removes a part from the parts list
    public void RemovePart(int id)
    {
        // First destroy the game object
        Destroy(parts[id].WorldAsset);

        // Then remove the part
        parts[id] = null;
    }

    // On hit trigger
    public void OnHit(ref Damagable other, float damage)
    {
        // Trigger each part's OnHit
        for (int i = 0; i < parts.Length; i++)
            if (parts[i] != null)
                parts[i].OnHit(ref _this, ref other, damage);
    }

    // Post hit trigger
    public void PostHit(ref Damagable other, float damage)
    {
        // Trigger each part's PostHit
        for (int i = 0; i < parts.Length; i++)
            if (parts[i] != null)
                parts[i].PostHit(ref _this, ref other, damage);
    }

    // Trigger game win condition
    public void GameWin()
    {
        // Make sure the game doesn't win for some reason after losing
        if (!GameEnded)
        {
            GameWon = true;
            GameEnded = true;
            GameObject.FindObjectOfType<GameUI>().WinGame();
            GameObject.FindObjectOfType<PlayerData>().GameEnded(true);
        }
    }

    // Trigger game lose condition
    public void GameLose()
    {
        // Make sure a lose event isn't triggered after the player has
        // won
        if (!GameWon)
        {
            GameEnded = true;
            GameObject.FindObjectOfType<GameUI>().LoseGame();
            GameObject.FindObjectOfType<PlayerData>().GameEnded(false);
        }
    }
}
