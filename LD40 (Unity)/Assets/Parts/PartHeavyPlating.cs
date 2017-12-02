using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartHeavyPlating : BasherPart
{

    // Basic test part:
    //
    /* -- Heavy Plating --
     * Reduces your basher's damage
     * taken by 10%.
     */

    /// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        // Heavy Plating is ID 0
        id = 0;

        // Decrease resistance by 0.1 (make the basher take 10% less)
        damagable.Resistance -= 0.1f;
    }
}
