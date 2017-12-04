using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantHealth : BasherPart {

    // Used to restore health on a timer
    float timer;

	/// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        timer = 0;

        ID = 2;
    }
	
	// Update is called once per frame
	public override void Update(ref Basher basher)
    {
        base.Update(ref basher);

		// Restore 0.5 health every second
        timer += Time.deltaTime;

        if (timer > 1)
        {
            damagable.Health += 0.5f;
            timer = 0;
        }
    }
}
