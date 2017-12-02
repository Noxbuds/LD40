using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantSacrifice : BasherPart {

	/// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        ID = 3;
    }
	
	// On impact, sacrifice some health to increase damage dealt
    public override void PostHit(ref Basher basher, ref Damagable target, float damage)
    {
        base.OnHit(ref basher, ref target, damage);

        // Let's say sacrifice 5% current hp to do 10% more damage
        damagable.Health *= 0.95f;
        target.Health -= damage * 0.1f; // resistance doesn't need to be factored in, it was already calculated with the initial damage calculation
    }
}
