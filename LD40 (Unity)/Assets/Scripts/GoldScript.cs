using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour {

    // To prevent receiving the reward multiple times...
    bool UsedUp;

    // Called when a collider touches the trigger
	void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.root.gameObject.name == "Basher" && !UsedUp)
        {
            UsedUp = true;
            GameObject.FindObjectOfType<PlayerData>().AwardGold(1);
            Destroy(this.gameObject);
        }
    }
}
