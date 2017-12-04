using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    // Just triggers a certain dialogue
    public string Scene;
    private bool Triggered;

    // Do it in update to guarantee that a dialogue manager will be present
	void Update()
    {
        if (!Triggered && !GameObject.FindObjectOfType<PlayerData>().playerData.TutorialsViewed[SceneToId(Scene)])
        {
            GameObject.FindObjectOfType<DialogueManager>().StartDialogue(Scene);
            Triggered = true;
        }
    }

    /// <summary>
    /// Get the ID of the scene in the player data's tutorials complete list
    /// </summary>
    /// <param name="Scene"></param>
    /// <returns></returns>
    public static int SceneToId(string Scene)
    {
        switch (Scene)
        {
            case "Shop":
                return 0;
            case "Level 1":
                return 1;
            case "Level 2":
                return 2;
            case "Level 3":
                return 3;

            default:
                return -1;
        }
    }
}
