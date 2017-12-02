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
    private List<BasherPart> parts;

    public float SpeedMultiplier;

	// Use this for initialization
	void Start ()
    {
        rb = this.GetComponent<Rigidbody>();
        _this = this.gameObject.GetComponent<Basher>();
        
        if (parts == null) parts = new List<BasherPart>();

        // Set default Damagable values for the basher
        Damagable dmgb = this.GetComponent<Damagable>();
        dmgb.Health = 100;
        dmgb.Resistance = 1;
        SpeedMultiplier = 1;

        // Add parts
        parts.Add(new PartHeavyPlating());
        
        // Go through parts and call the Init function
        for (int i = 0; i < parts.Count; i++)
            parts[i].Initialise(ref _this);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // On hit trigger
    public void OnHit(ref Damagable other)
    {
        // Trigger each part's OnHit
        for (int i = 0; i < parts.Count; i++)
            parts[i].OnHit(ref _this, ref other);
    }
}
