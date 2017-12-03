using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Basher))]
public class BasherController : MonoBehaviour
{

    // Input handler for the battering ram
    Rigidbody2D rb;
    Basher basher;

    // Key bindings:
    public KeyCode Left;
    public KeyCode Right;

    // Force applied when moving
    float MovingForce = 20;
    public HingeJoint2D[] Wheels;

	// Use this for initialization
	void Start ()
    {
        rb = this.GetComponent<Rigidbody2D>();
        basher = this.GetComponent<Basher>();

        // just use some pre defined key bindings for now
        Left = KeyCode.A;
        Right = KeyCode.D;
	}

	// Update is called once per frame
	void Update ()
    {
        // Check input and add forces
        if (Input.GetKey(Left))
            // Using hinge joints to hold the wheels in place, and using
            // their rigidbodies' torque mechanics to move the basher
            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i].GetComponent<Rigidbody2D>().AddTorque(10);
            }
        else if (Input.GetKey(Right))
            // Using hinge joints to hold the wheels in place, and using
            // their rigidbodies' torque mechanics to move the basher
            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i].GetComponent<Rigidbody2D>().AddTorque(-10);
            }
	}
}
