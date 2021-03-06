﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Basher))]
public class BasherController : MonoBehaviour
{

    // Input handler for the battering ram

    // Key bindings:
    public KeyCode Left;
    public KeyCode Right;

    // Force applied when moving
    public HingeJoint2D[] Wheels;

	// Use this for initialization
	void Start ()
    {
        // just use some pre defined key bindings for now
        Left = KeyCode.A;
        Right = KeyCode.D;
	}

	// Update is called once per frame
	void Update ()
    {
        // Only accept control
        if (!GameObject.FindObjectOfType<Basher>().GameEnded)
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

        // Just go to main menu on pressing escape. Don't want to spend
        // a lot of time trying to make a pretty pause menu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
            GameObject.FindObjectOfType<PlayerData>().SaveProgress();
        }
	}
}
