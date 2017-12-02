using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour {

    // 0 - 3 for now..
    public int SizeTier;

    // Combined health pool of all obstacles
    public float TotalHealth;

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

	// Use this for initialization
	void Start () {
        // Parameters above should be assigned in editor
        FireTimer = 0;

        foreach (Damagable d in GameObject.FindObjectsOfType<Damagable>())
        {
            if (d.IsEnemy)
                TotalHealth += d.Health;
        }
	}
	
	// Update is called once per frame
	void Update () {
        FireTimer += Time.deltaTime;

        if (FireTimer >= 10f / (float)NumberOfArchers)
        {
            FireTimer = 0;
            ShootPlayer(2);
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
