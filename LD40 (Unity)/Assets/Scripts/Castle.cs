using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour {

    // 0 - 3 for now..
    public int SizeTier;

    // Combined health pool of all obstacles
    public float TotalHealth;
    public float MaxHealth;

    // Number of mages:
    // 0 = no effect
    // 1 = 0.8 * enchantment
    // 2 = 0.4 * enchantment
    // 3 = 0.2 * enchantment
    public int NumberOfMages;

    // Number of archers:
    // 0 = no arrows
    // 1 = 2 damage every 10 seconds
    // 2 = 2 damage every 5 seconds
    // 3 = 2 damage every 2.5 seconds
    // 4 = 2 damage every 1.25 seconds...
    public int NumberOfArchers;
    private float FireTimer;

    private List<Damagable> Allies;

	// Use this for initialization
	void Start () {
        // Initialise the Allies list
        Allies = new List<Damagable>();
        
        // Parameters above should be assigned in editor
        FireTimer = 0;

        foreach (Damagable d in GameObject.FindObjectsOfType<Damagable>())
        {
            if (d.IsEnemy)
            {
                TotalHealth += d.Health;
                Allies.Add(d);
            }
        }

        // Set max health for UI calculations
        MaxHealth = TotalHealth;
	}
	
	// Update is called once per frame
	void Update () {
        // Calculate new health
        float newHealth = 0f;

        for (int i = 0; i < Allies.Count; i++)
        {
            newHealth += Allies[i].Health;
        }

        if (TotalHealth < 0)
            GameObject.FindObjectOfType<Basher>().GameWin();

        // Only do stuff if the castle is still alive
        if (TotalHealth > 0)
        {
            // Assign the health to the new health after it's calculated
            TotalHealth = newHealth;

            // Add to the archer firing timer
            FireTimer += Time.deltaTime;

            if (FireTimer >= 10f / (float)NumberOfArchers)
            {
                FireTimer = 0;
                ShootPlayer(2);
            }
        }
	}

    /// <summary>
    /// Shoot the player.
    /// </summary>
    void ShootPlayer(float damage)
    {
        // Damage the player
        GameObject.FindObjectOfType<Basher>().GetComponent<Damagable>().Health -= damage;
    }
}
