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
        ResistMults = new float[MaxParts];

        // Set default Damagable values for the basher
        Damagable dmgb = this.GetComponent<Damagable>();
        dmgb.Health = 100;
        dmgb.Resistance = 1;
        SpeedMultiplier = 1;

        // Fetch part data from player data
        
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

            // Make a 'running total' of the resistance
            this.GetComponent<Damagable>().Resistance *= ResistMults[i];
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
            Time.timeScale = 0.05f;
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
            Time.timeScale = 0.05f;
            GameObject.FindObjectOfType<GameUI>().LoseGame();
            GameObject.FindObjectOfType<PlayerData>().GameEnded(false);
        }
    }
}
