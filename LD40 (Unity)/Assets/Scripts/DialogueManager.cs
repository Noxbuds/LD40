using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static Dialogue[] Dialogues =
    {
        // Shop dialogue
        new Dialogue("Shop", "Welcome to the Part Shop. Here, you can purchase parts for your Basher which modify it during a level."),
        new Dialogue("Shop", "There are two types - Normal Parts, which usually have simple buffs, such as reducing damage taken or increasing your Basher's speed; and Enchantments, which grant stronger effects - however, you can only use 1 Enchantment at a time, while you can use 2 Normal Parts at once."),
        new Dialogue("Shop", "You can equip or unequip the parts once you have bought them for gold. Remember - you can have 2 parts and 1 enchantment active at the same time."),

        // Level 1 dialogue
        new Dialogue("Level 1", "Welcome to Basher Builder! You must control a Basher in order to destroy a Castle Wall."),
        new Dialogue("Level 1", "If you look at the bottom of your screen, you will see two coloured bars; the green one represents your Basher's health, while the red one represents the Wall's health."),
        new Dialogue("Level 1", "When your Basher's health reaches 0, you have lost the level. However, if you can get the Wall's health to 0 while keeping your Basher alive, you win the level."),
        new Dialogue("Level 1", "The faster your Basher is going when it collides with an object, the more damage you will do to it - but watch out, you will also take more damage the faster you travel!"),
        new Dialogue("Level 1", "In the top left of your screen you will see a golden coin with a number next to it - that is how much gold you have. You can pick up gold coins during levels, and later on you can use them to buy special parts to level up your Basher!"),
        new Dialogue("Level 1", "In the castle, there are some Archers - be careful, they will shoot your Basher every few seconds. As you progress through the game, the castles will start to have more and more archers - the more there are, the faster they shoot at you."),

        // Level 2 dialogue
        new Dialogue("Level 2", "You will notice in this level that there's a pile of stones the enemy has set up to try slow you down. You will have to destroy them by hitting them - but beware, you will also take damage from it!")
    };

    // The current dialogue counter
    public int DialogueCounter;
    public int CurrentMaxDialogues;
    public string[] CurrentDialogues;
    public static int TutorialCount = 3;

    private bool _ShowingDialogue;
    public bool ShowingDialogue
    {
        get { return _ShowingDialogue; }
        set
        {
            // If it's changing and it's being set to false,
            // we want to change the 'tutorial complete' status
            // in the player data
            if (_ShowingDialogue != value && !value)
            {
                GameObject.FindObjectOfType<PlayerData>().playerData.TutorialsViewed[DialogueTrigger.SceneToId(CurrentScene)] = true;
                GameObject.FindObjectOfType<PlayerData>().SaveProgress();
            }

            // Set the value
            _ShowingDialogue = value;
        }
    }

    private string _CurrentScene;
    public string CurrentScene
    {
        get { return _CurrentScene; }
        set
        {
            // Only need to loop through the array and calculate
            // when the scene changes
            CurrentMaxDialogues = GetDialogueCount(value);
            CurrentDialogues = GetDialoguesForScene(value);
            _CurrentScene = value;
        }
    }

    // GUI stuff
    private GUIStyle TextStyle;
    public Font TextFont;
    public Texture2D ItemInfoImg;

    /// <summary>
    /// Triggers this level's dialogue with a certain scene
    /// </summary>
    void StartDialogues(string Scene)
    {
        ShowingDialogue = true;
        CurrentScene = Scene;
    }

    void Start()
    {
        TextStyle = new GUIStyle();
        TextStyle.font = TextFont;
    }

    void Update()
    {
        // Increment dialogue counter when pressing space
        if (ShowingDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                DialogueCounter++;

            // Just make it so while a dialogue is playing, nothing happens
            Time.timeScale = 0.0f;
        }
        else
            Time.timeScale = 1.0f;

        // If the dialogue counter is higher than the amount
        // of dialogues for the scene, stop showing
        if (DialogueCounter >= CurrentMaxDialogues) // since we start at 0 not 1
            ShowingDialogue = false;
    }

    /// <summary>
    /// Starts the dialogue for a scene
    /// </summary>
    public void StartDialogue(string Scene)
    {
        CurrentScene = Scene;
        DialogueCounter = 0;
        ShowingDialogue = true;
    }

    void OnGUI()
    {
        // Draw the current dialogue
        if (ShowingDialogue && DialogueCounter < CurrentMaxDialogues)
        {
            // Ripped the code from my main menu's item display thing... let's hope it works :s
            float ItemDisplayScale = Screen.width / ItemInfoImg.width;

            // Draw the dialogue panel
            GUI.DrawTexture(new Rect(0, 0, Screen.width, ItemInfoImg.height * ItemDisplayScale), ItemInfoImg);

            // Display dialogue
            float DescriptionYP = 8 * ItemDisplayScale + Screen.height * 0.04f;
            TextStyle.fontSize = (int)(Screen.height * 0.04f); // correct the font size

            TextStyle.wordWrap = true;
            GUI.Label(new Rect(19 * ItemDisplayScale, DescriptionYP, ItemInfoImg.width * ItemDisplayScale - 19 * ItemDisplayScale, Screen.height * 0.04f), CurrentDialogues[DialogueCounter] + "\n(press space to continue...)", TextStyle);
        }
    }

    /// <summary>
    /// Get an array of all the dialogues for a certain scene
    /// </summary>
    /// <returns></returns>
    string[] GetDialoguesForScene(string Scene)
    {
        List<string> LevelDialogues = new List<string>();

        // Order doesn't really matter....
        for (int i = 0; i < Dialogues.Length; i++)
            if (Dialogues[i].Scene == Scene)
                LevelDialogues.Add(Dialogues[i].Text);

        // Return as a string[]
        return LevelDialogues.ToArray();
    }

    /// <summary>
    /// Get the number of dialogues for a scene
    /// </summary>
    /// <returns></returns>
    int GetDialogueCount(string Scene)
    {
        return GetDialoguesForScene(Scene).Length;
    }

    /// <summary>
    /// Stop showing the tutorial without saving it as completed
    /// </summary>
    public void NTStopShowing()
    {
        _ShowingDialogue = false;
    }
}

[System.Serializable]
public class Dialogue
{
    public string Scene;
    public string Text;

    public Dialogue(string Scene, string Text)
    {
        this.Scene = Scene;
        this.Text = Text;
    }
}