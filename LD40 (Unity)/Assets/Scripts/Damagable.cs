using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    private Rigidbody rb;

    // Public stat handlers
    public float Health
    {
        get { return _Health; }
        set
        {
            // Triggers or events etc

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
        rb = this.GetComponent<Rigidbody>();

        // Trigger-less value setting
        if (_Health == 0) _Health = 100;

        if (_Damage == 0) _Damage = 1;
        if (_Speed == 0) _Speed = 1;
        if (_Resistance == 0) _Resistance = 1;
    }

    // Update speed
    void Update()
    {
        Speed = rb.velocity.sqrMagnitude;
    }

	// Collision detection
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is a Damagable
        if (collision.collider.gameObject.GetComponent<Damagable>() != null)
        {
            // Handle the Damagable effects
            Damagable other = collision.collider.gameObject.GetComponent<Damagable>();

            HandleCollision(other);
        }
        else if (collision.collider.transform.parent.GetComponent<Damagable>() != null)
        {
            // for objects who have their colliders as children
            Damagable other = collision.collider.transform.parent.GetComponent<Damagable>();

            HandleCollision(other);
        }
    }

    void HandleCollision(Damagable other)
    {
        // Speed multiplier:
        // y = log(x) + 0.0005x^2

        // Deal damage - only have to do this here since the other
        // damagable will also have this event trigger and will take away the other
        // object's health

        // Extra triggers for the battering ram
        if (other.gameObject.GetComponent<Basher>() != null)
            other.gameObject.GetComponent<Basher>().OnHit(ref other);

        // Walls' damage will scale based on the other object's speed
        if (this.gameObject.name == "Wall")
            other.Health -= Damage * other.Resistance + Damage * (Mathf.Log(other.Speed) * 0.0005f * Mathf.Pow(other.Speed, 2.0f));
        else
            other.Health -= Damage * other.Resistance + Damage * (Mathf.Log(Speed) * 0.0005f * Mathf.Pow(Speed, 2.0f));
    }
}
