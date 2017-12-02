using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHeavyShield : BasherPart
{

    // Basic test part:
    //
    /* -- Heavy Shield --
     * Shields your Basher from
     * a total of 50 damage, then
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

        Health = 50;
    }

    // Take away this shield's health instead
    public override void OnHit(ref Basher basher, ref Damagable target, float damage)
    {
        base.OnHit(ref basher, ref target, damage);

        // Heal the basher before it takes damage
        damagable.Health += damage;

        // Take the damage away from this shield's health
        Health -= damage;

        // If the shield's health is below 0, deal the excess to the basher
        if (Health < 0)
            damagable.Health += Health;

        Debug.Log(Health);

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
