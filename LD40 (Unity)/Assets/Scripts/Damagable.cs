using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Damagable : MonoBehaviour
{

    // Script for anything that is damagable
    // will need to modify in a bit to make damage
    // scale with other object's speed

    // Private/local stat values
    [SerializeField]
    private float _Health;
    [SerializeField]
    private float _Resistance;
    [SerializeField]
    private float _Damage;
    [SerializeField]
    protected float _Speed;
    private Rigidbody2D rb;

    // Whether or not this belongs to the enemy castle
    public bool IsEnemy;
    float deathTimer; // something else for enemies

    // Public stat handlers
    public float Health
    {
        get { return _Health; }
        set
        {
            // Triggers or events etc
            
            // Cause the rigidbody to be movable and lose
            // its collider
            if (this.gameObject.name != "Basher" && value < 0)
            {
                rb.isKinematic = false;
                
                // Remove any colliders on this
                if (this.GetComponent<Collider2D>() != null)
                    Destroy(this.GetComponent<Collider2D>());

                // Remove any colliders in children
                foreach (Collider2D c in this.GetComponentsInChildren<Collider2D>())
                    Destroy(c);
            }

            // Set value
            _Health = value;
        }
    } // base hp
    public float Resistance
    {
        get { return _Resistance; }
        set
        {
            // Triggers/event hooks

            // Set value
            _Resistance = value;
        }
    } // base resistance - damage will be multiplied by this
    public float Damage
    {
        get { return _Damage; }
        set
        {
            // Triggers/event hooks

            // Set value
            _Damage = value;
        }
    } // base damage
    public float Speed
    {
        get { return _Speed; }
        set
        {
            // Only change if speed is greater than a set value
            if (value > 0.5f)
                _Speed = value;
        }
    } // speed before collisions

    // Set a default value for health
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        // Trigger-less value setting
        if (_Health == 0) _Health = 100;

        if (_Damage == 0) _Damage = 1;
        if (_Speed == 0) _Speed = 1;
        if (_Resistance == 0) _Resistance = 1;

        // If this is not the basher, we want the object to be
        // unmovable until it's destroyed
        if (this.gameObject.name != "Basher")
            rb.isKinematic = true;
    }

    // Update speed
    void Update()
    {
        Speed = rb.velocity.sqrMagnitude;
    }

	// Collision detection
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name != "wheel" && this.gameObject.name != "wheel")
        {
            // Check if the collider is a Damagable
            if (collision.collider.gameObject.GetComponent<Damagable>() != null)
            {
                // Handle the Damagable effects
                Damagable other = collision.collider.gameObject.GetComponent<Damagable>();

                HandleCollision(other);
            }
            else if (collision.collider.transform.parent != null) // fixes an error that arose when putting the terrain's collider onto the parent
            {
                if (collision.collider.transform.parent.GetComponent<Damagable>() != null)
                {
                    // for objects who have their colliders as children
                    Damagable other = collision.collider.transform.parent.GetComponent<Damagable>();

                    HandleCollision(other);
                }
            }
        }
    }

    void HandleCollision(Damagable other)
    {
        // Speed multiplier:
        // y = log(x) + 0.0005x^2

        // Deal damage - only have to do this here since the other
        // damagable will also have this event trigger and will take away the other
        // object's health

        Damagable _this = this.gameObject.GetComponent<Damagable>();

        float damageDealt = 0;

        // Pre-calculate damage for parts
        if (this.gameObject.name == "Wall")
            damageDealt = Damage + Damage * (Mathf.Log(other.Speed) + 0.0005f * Mathf.Pow(other.Speed, 2.0f));
        else
            damageDealt = Damage + Damage * (Mathf.Log(Speed) + 0.0005f * Mathf.Pow(Speed, 2.0f));

        // Pre-hit triggers for the battering ram
        if (other.gameObject.GetComponent<Basher>() != null)
            other.gameObject.GetComponent<Basher>().OnHit(ref _this, damageDealt);

        // Walls' damage will scale based on the other object's speed
        other.Health -= damageDealt * other.Resistance;

        // Post-hit triggers for battering ram
        if (other.gameObject.GetComponent<Basher>() != null)
            other.gameObject.GetComponent<Basher>().PostHit(ref _this, damageDealt);
    }
}
