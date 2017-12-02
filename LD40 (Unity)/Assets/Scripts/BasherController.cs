﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Basher))]
public class BasherController : MonoBehaviour
{

    // Input handler for the battering ram
    Rigidbody rb;
    Basher basher;

    // Key bindings:
    public KeyCode Left;
    public KeyCode Right;

    // Force applied when moving
    float MovingForce = 20;

	// Use this for initialization
	void Start ()
    {
        rb = this.GetComponent<Rigidbody>();
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
        {
            // How the hell big do these forces need to be ?!
            rb.AddForce(-MovingForce * basher.SpeedMultiplier * basher.transform.forward);
        }
        else if (Input.GetKey(Right))
        {
            rb.AddForce(MovingForce * basher.SpeedMultiplier * basher.transform.forward);
        }
	}
}
