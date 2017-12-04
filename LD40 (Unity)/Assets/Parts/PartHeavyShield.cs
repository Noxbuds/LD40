using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHeavyShield : BasherPart
{

    // Basic test part:
    //
    /* -- Heavy Shield --
     * Shields your Basher from
     * a total of 10 damage, then
     * breaks
     */

    public float Health;
    private float tempResist;

    /// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        // Heavy Shield is ID 1
        ID = 1;

        Health = 10;
    }

    // Take away this shield's health instead
    public override void OnHit(ref Basher basher, ref Damagable target, float damage)
    {
        base.OnHit(ref basher, ref target, damage);

        // If the damage is less than the shield's remaining health, add the
        // damage to the basher's health. This will effectively bring the damage
        // taken to 0.
        if (damage < Health)
        {
            damagable.Health += damage;
            Health -= damage;
        }

        // If the damage is greater than the shield's remaining health, just add all
        // of that to the basher's health and remove it
        if (damage > Health)
        {
            damagable.Health += Health;
            Health = 0;
        }

        // Destroy the shield if its HP goes below 0
        if (Health <= 0)
        {
            // Find this part's ID in the basher parts list
            int idToRemove = -1;
            for (int i = 0; i < basher.parts.Length; i++)
                if (basher.parts[i] == this)
                {
                    idToRemove = i;
                    break;
                }

            // Remove this from the parts list
            basher.RemovePart(idToRemove);
        }
    }
}
