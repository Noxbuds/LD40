using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Damagable : MonoBehaviour {

    // Script for anything that is damagable
    // will need to modify in a bit to make damage
    // scale with other object's speed

    // Private/local stat values
    private float _Health;
    private float _Resistance;
    private float _Damage;
    protected float _Speed;

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

	// Collision detection
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is a Damagable
        if (collision.collider.gameObject.GetComponent<Damagable>() != null)
        {
            // Handle the Damagable effects
            Damagable other = collision.collider.gameObject.GetComponent<Damagable>();

            // Speed multiplier:
            // y = log(x) + 0.0005x^2

            // Deal damage - only have to do this here since the other
            // damagable will also have this event trigger and will take away the other
            // object's health
            other.Health -= Damage * other.Resistance + Damage * (Mathf.Log(other.Speed) * 0.0005f * Mathf.Pow(other.Speed, 2.0f));
        }
    }
}
