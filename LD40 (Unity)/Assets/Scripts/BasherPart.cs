using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherPart {

    protected Damagable damagable;

    /// <summary>
    /// Triggered on startup to modify things
    /// </summary>
    public virtual void Initialise(ref Basher basher) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered whenever the basher collides with an object
    /// </summary>
    public virtual void OnHit(ref Basher basher, ref Damagable target) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered whenever the basher is damaged (e.g from archers)
    /// </summary>
    public virtual void OnDamage(ref Basher basher) { damagable = basher.GetComponent<Damagable>(); }
}
