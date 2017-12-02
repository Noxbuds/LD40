using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Basher))]
public class BasherController : MonoBehaviour {

    // Input handler for the battering ram
    Rigidbody rb;

    // Key bindings:
    public KeyCode Left;
    public KeyCode Right;

    // Wheels - needed to make them move freely
    public WheelCollider[] wheels;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();

        // just use some pre defined key bindings for now
        Left = KeyCode.A;
        Right = KeyCode.D;

        // fix jittering with wheel collider
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].ConfigureVehicleSubsteps(10, 10, 10);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Check input and add forces
        // TODO: Make a maximum speed
        if (Input.GetKey(Left))
        {
            //rb.AddRelativeForce(new Vector3(-10000000.0f * rb.mass, 0.0f, 0.0f));
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = -1000;
            }
        }
        else if (Input.GetKey(Right))
        {
            //rb.AddRelativeForce(new Vector3(10000000.0f * rb.mass, 0.0f, 0.0f));
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 1000;
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
	}
}
