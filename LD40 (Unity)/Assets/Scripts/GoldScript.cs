using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldScript : MonoBehaviour {

    // To prevent receiving the reward multiple times...
    bool UsedUp;

    void Update()
    {
        transform.GetChild(0).transform.Translate(new Vector3(0, Mathf.Sin(Time.time * 2) * 0.01f, 0));
    }

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
