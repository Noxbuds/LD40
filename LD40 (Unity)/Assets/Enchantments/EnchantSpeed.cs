using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantSpeed : BasherPart
{
    // For changing resistance every frame
    float lastResistance;

    /// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        ID = 4;
    }

    // Update is called once per frame
    public override void Update(ref Basher basher)
    {
        // Increase the resistance by the speed
        float resistAmount = 1f - Mathf.Log(damagable.Speed) * 0.03f;

        if (resistAmount < 0.1f)
            resistAmount = 0.1f;

        // Find this part's ID in the basher parts list
        int partID = -1;
        for (int i = 0; i < basher.enchantments.Length; i++)
            if (basher.enchantments[i] == this)
            {
                partID = i;
                break;
            }

        if (partID != -1)
            basher.ResistMults[Basher.MaxParts + partID] = resistAmount;
    }
}
