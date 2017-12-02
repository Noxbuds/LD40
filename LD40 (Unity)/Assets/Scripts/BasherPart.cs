using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherPart
{
    public enum Type { Part, Enchantment };

    // Base class for a basher part
    protected Damagable damagable;
    public int ID;
    public Type PartType;

    // Stuff for displaying on the basher
    public GameObject WorldPrefab;
    public GameObject WorldAsset;

    /// <summary>
    /// Triggered on startup to modify things
    /// </summary>
    public virtual void Initialise(ref Basher basher) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered whenever the basher collides with an object
    /// </summary>
    public virtual void OnHit(ref Basher basher, ref Damagable target, float damage) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered whenever the basher is damaged (e.g from archers)
    /// </summary>
    public virtual void OnDamage(ref Basher basher, float damage) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered after the basher collides with an object and has taken damage
    /// </summary>
    public virtual void PostHit(ref Basher basher, ref Damagable target, float damage) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Triggered after the basher is damaged (e.g from archers) and has taken the damage
    /// </summary>
    public virtual void PostDamage(ref Basher basher, float damage) { damagable = basher.GetComponent<Damagable>(); }

    /// <summary>
    /// Called every frame by the Basher
    /// </summary>
    public virtual void Update(ref Basher basher) { damagable = basher.GetComponent<Damagable>();  }

    // Spawns the prefab in the world
    public void SpawnPrefab()
    {
        // Attempt to fetch the model prefab
        PartDatabase partDB = (PartDatabase)GameObject.FindObjectOfType<PartDatabase>(); // there'll only ever be one part database in the scene
        WorldPrefab = partDB.RetrievePrefab(ID);

        // Initialise the world prefab if it's assigned
        if (WorldPrefab != null)
        {
            WorldAsset = (GameObject)GameObject.Instantiate(WorldPrefab);
            WorldAsset.transform.parent = GameObject.Find("Basher").transform;
            WorldAsset.transform.localPosition = Vector3.zero;
        }
    }
}
