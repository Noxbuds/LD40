using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
public class Basher : MonoBehaviour
{
    // The main script that controls the battering ram

    // Local references
    private Rigidbody rb;
    private Basher _this;
    public BasherPart[] parts;

    public float SpeedMultiplier;
    public int MaxParts;
    public int MaxEnchantments;

	// Use this for initialization
	void Start ()
    {
        rb = this.GetComponent<Rigidbody>();
        _this = this.gameObject.GetComponent<Basher>();
        
        // Assign max parts and enchantments
        MaxParts = 3;
        MaxEnchantments = 1;

        if (parts == null) parts = new BasherPart[MaxParts];

        // Set default Damagable values for the basher
        Damagable dmgb = this.GetComponent<Damagable>();
        dmgb.Health = 100;
        dmgb.Resistance = 1;
        SpeedMultiplier = 1;

        // Add parts
        parts[0] = new PartHeavyShield();
        
        // Go through parts and call the Init function
        for (int i = 0; i < parts.Length; i++)
            if (parts[i] != null)
                parts[i].Initialise(ref _this);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
}
