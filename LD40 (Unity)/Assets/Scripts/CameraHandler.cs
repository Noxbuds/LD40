using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    // The transform of the basher
    public Transform BasherTrns;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = this.transform.position;
        newPos.x = BasherTrns.transform.position.x;
        newPos.y = 0.7f * BasherTrns.transform.position.y;
        this.transform.position = newPos;
	}
}
