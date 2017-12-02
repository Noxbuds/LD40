using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherPart
{
    // Base class for a basher part
    protected Damagable damagable;
    public int id;

    // Stuff for displaying on the basher
    public GameObject WorldPrefab;
    public GameObject WorldAsset;

    // Constructor
    public BasherPart()
    {
        // Initialise the world prefab if it's assigned
        if (WorldPrefab != null)
        {
            WorldAsset = (GameObject) GameObject.Instantiate(WorldPrefab);
            WorldAsset.transform.parent = GameObject.Find("Basher").transform;
            WorldAsset.transform.localPosition = Vector3.zero;
        }
    }

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
