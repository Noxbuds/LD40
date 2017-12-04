using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    /* Menu IDs:
     * 
     * 0 = Main Menu
     * 1 = Level Select
     * 2 = Parts
     * 3 = Options
     */
    public int MenuID;

    // Data about what's on screen
    private bool ShowGold;
    private int SelectedPart;
    private int PartPage;

    // UI Elemeents
    public Texture2D GoldIcon;
    public Texture2D Background;
    public Texture2D ButtonImg;
    public Texture2D BackButtonImg;
    public Texture2D ItemInfoImg;
    public Texture2D MinusImg;
    public Texture2D PlusImg;
    public Font GameFont;
    public GUIStyle TextStyle;
    public GUIStyle TitleStyle;
    public GUIStyle ButtonStyle; // create in editor
    public GUIStyle BackButtonStyle; // create in editor
    public GUIStyle EquipButtonStyle; // create in editor
    public GUIStyle ForwardButtonStyle; // create in editor
    public GUIStyle PlusButtonStyle;
    public GUIStyle MinusButtonStyle;
    public GUIStyle SettingBoxStyle;

	// Don't need to load at startup; the player data handler already does this
    void Start()
    {
        TextStyle = new GUIStyle();
        TextStyle.font = GameFont;

        TitleStyle = new GUIStyle();
        TitleStyle.font = GameFont;
        TitleStyle.alignment = TextAnchor.MiddleCenter;

        // Make a special GUI Style for box
        // this won't have hover/active
        SettingBoxStyle = new GUIStyle();
        SettingBoxStyle.alignment = TextAnchor.MiddleCenter;
        SettingBoxStyle.normal.background = ButtonStyle.normal.background;
        SettingBoxStyle.font = ButtonStyle.font;
    }

    // GUI Code (Yes the legacy GUI system is slower and can be CPU intense,
    // but I've never taken the time to learn the 'new' one...)
    void OnGUI()
    {
        // Scale for the buttons
        float ButtonScale = (Screen.width * 0.276f) / ButtonImg.width;
        ButtonStyle.fontSize = (int)(Screen.height * 0.04f);

        // Just in case...
        TextStyle.wordWrap = false;

        // If we haven't yet seen the tutorial for the shop, let's show it
        DialogueManager dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
        if (MenuID != 2)
        {
            dialogueManager.NTStopShowing();
        }

        // Draw menu elements
        switch (MenuID)
        {
            case 0: // Main Menu
                ShowGold = false;

                // Same code as the shop item panel pretty much
                float ItemDisplayScale = Screen.width / ItemInfoImg.width;

                // Draw the dialogue panel
                GUI.DrawTexture(new Rect(0, 0, Screen.width, ItemInfoImg.height * ItemDisplayScale), ItemInfoImg);

                // Display dialogue
                float DescriptionYP = 8 * ItemDisplayScale + Screen.height * 0.04f;
                TitleStyle.fontSize = (int)(Screen.height * 0.08f); // correct the font size

                TitleStyle.wordWrap = true;
                GUI.Label(new Rect(19 * ItemDisplayScale, DescriptionYP, ItemInfoImg.width * ItemDisplayScale - 19 * ItemDisplayScale, Screen.height * 0.04f), "Basher Builder", TitleStyle);

                // Fixed positions
                float leftX = 41 * ButtonScale;
                float rightX = 101 * ButtonScale;
                float topY = 43 * ButtonScale;
                float bottomY = 70 * ButtonScale;

                // Play button
                if (GUI.Button(new Rect(leftX, topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Play", ButtonStyle))
                    MenuID = 1;

                // Parts button
                if (GUI.Button(new Rect(rightX, topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Parts", ButtonStyle))
                {
                    // If we haven't yet seen the tutorial for the shop, let's show it
                    if (!GameObject.FindObjectOfType<PlayerData>().playerData.TutorialsViewed[0])
                    {
                        dialogueManager.StartDialogue("Shop");
                    }
                    MenuID = 2;
                }

                // Options button
                if (GUI.Button(new Rect(leftX, bottomY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Settings", ButtonStyle))
                    MenuID = 3;

                // Quit button
                if (GUI.Button(new Rect(rightX, bottomY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Quit", ButtonStyle))
                {
                    GameObject.FindObjectOfType<PlayerData>().SaveProgress();
                    Application.Quit();
                }

                break;
            case 1: // Level Select
                ShowGold = true;
                
                // 6 levels per screen? Obviously only 3 levels atm but...
                // 3x2 level display.
                float X1 = 11 * ButtonScale;
                float X2 = 71 * ButtonScale;
                float X3 = 131 * ButtonScale;
                topY = 43 * ButtonScale;
                bottomY = 70 * ButtonScale;

                // Fetch the level completion list from the player data
                bool[] LevelsComplete = GameObject.FindObjectOfType<PlayerData>().playerData.LevelsWon;

                for (int i = 0; i < 6; i++)
                {
                    if (i < LevelsComplete.Length)
                    {
                        // The index of the X position to use; 1, 2 or 3
                        int xi = (i > 2) ? i - 3 : i;
                        float ThisX = (xi == 0) ? X1 : ((xi == 1) ? X2 : X3); // if xi is 0, ThisX = X1, otherwise if xi = 1, ThisX = X2, otherwise ThisX = X3

                        // Draw the button
                        if (GUI.Button(new Rect(ThisX, i > 2 ? bottomY : topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Level " + i + " (" + (LevelsComplete[i] ? "Done" : "Not done") + ")", ButtonStyle))
                        {
                            // You can only start the level if you have completed the previous one
                            if (i == 0)
                                SceneManager.LoadScene("Level " + (i + 1).ToString());
                            if (i > 0)
                                if (LevelsComplete[i - 1])
                                    SceneManager.LoadScene("Level " + (i + 1).ToString());
                        }
                    }
                }

                break;
            case 2: // Parts
                ShowGold = true; // for sure

                // Same style as the level selection
                // 3x2 display of parts. Will also need a forward/backward button...
                X1 = 11 * ButtonScale;
                X2 = 71 * ButtonScale;
                X3 = 131 * ButtonScale;
                topY = 43 * ButtonScale;
                bottomY = 70 * ButtonScale;

                PartDatabase partDB = GameObject.FindObjectOfType<PartDatabase>();
                ItemInfo[] partList = partDB.ItemInfoList;

                // Get a local copy of player data
                PlayerData.SPData playerData = GameObject.FindObjectOfType<PlayerData>().playerData;
                EquipButtonStyle.fontSize = (int)(Screen.height * 0.04f);
                bool[] partsOwned = playerData.PartsOwned;

                // Draw the 3x2 layout
                for (int i = 0; i < 6; i++)
                {
                    if (i + (PartPage * 6) < partList.Length)
                    {
                        // The index of the X position to use; 1, 2 or 3
                        int xi = (i > 2) ? i - 3 : i;
                        float ThisX = (xi == 0) ? X1 : ((xi == 1) ? X2 : X3); // if xi is 0, ThisX = X1, otherwise if xi = 1, ThisX = X2, otherwise ThisX = X3

                        // Draw the button
                        if (GUI.Button(new Rect(ThisX, i > 2 ? bottomY : topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), partList[i + (PartPage * 6)].ItemName + " " + (IsPartEquipped(i, playerData) ? "(*)" : ""), ButtonStyle))
                            SelectedPart = i + (PartPage * 6);
                    }
                }

                // Only show the item panel once the shop tutorial has been shown
                // Draw the item info
                ItemDisplayScale = Screen.width / ItemInfoImg.width;

                if (GameObject.FindObjectOfType<PlayerData>().playerData.TutorialsViewed[0])
                {

                    // Draw the item info panel
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, ItemInfoImg.height * ItemDisplayScale), ItemInfoImg);

                    // Display the item info
                    string typeString = partList[SelectedPart].IsEnchantment ? " (Enchantment)" : " (Normal Part)";

                    GUI.Label(new Rect(19 * ItemDisplayScale, 8 * ItemDisplayScale, ItemInfoImg.width * ItemDisplayScale - 19 * ItemDisplayScale, Screen.height * 0.04f), partList[SelectedPart].ItemName + typeString, TextStyle);

                    // Display item description
                    DescriptionYP = 8 * ItemDisplayScale + Screen.height * 0.04f;

                    TextStyle.wordWrap = true;
                    GUI.Label(new Rect(19 * ItemDisplayScale, DescriptionYP, ItemInfoImg.width * ItemDisplayScale - 19 * ItemDisplayScale, Screen.height * 0.04f), partList[SelectedPart].ItemDescription, TextStyle);
                }

                // Equip/buy buttons
                if (partsOwned[SelectedPart])
                {
                    // Check if the part is equipped yet
                    if (IsPartEquipped(SelectedPart, playerData))
                    {
                        if (GUI.Button(new Rect(138 * ItemDisplayScale, 27 * ItemDisplayScale, 18 * ItemDisplayScale, 8 * ItemDisplayScale), "Unequip", EquipButtonStyle))
                        {
                            // Unequip it
                            UnequipItem(SelectedPart, playerData);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(138 * ItemDisplayScale, 27 * ItemDisplayScale, 18 * ItemDisplayScale, 8 * ItemDisplayScale), "Equip", EquipButtonStyle))
                        {
                            // Check if there's space, and equip it if so
                            if (IsSpaceInParts(SelectedPart, playerData))
                                EquipItem(SelectedPart, playerData);
                        }
                    }
                }
                else
                    if (GUI.Button(new Rect(138 * ItemDisplayScale, 27 * ItemDisplayScale, 18 * ItemDisplayScale, 8 * ItemDisplayScale), "Buy (" + partList[SelectedPart].GoldCost + "g)", EquipButtonStyle))
                    {
                        // Check if the player can afford it
                        if (playerData.Gold >= partList[SelectedPart].GoldCost)
                        {
                            // Subtract the cost
                            playerData.Gold -= partList[SelectedPart].GoldCost;

                            // Set the parts owned status to true
                            partsOwned[SelectedPart] = true;
                            GameObject.FindObjectOfType<PlayerData>().playerData.PartsOwned[SelectedPart] = true;

                            // Remember to save progress after each equip/unequip/purchase
                            GameObject.FindObjectOfType<PlayerData>().SaveProgress();
                        }
                    }

                // Backward button for page
                if (GUI.Button(new Rect(81 * ButtonScale, 100 * ButtonScale, 13 * ButtonScale, 7 * ButtonScale), "", BackButtonStyle))
                    if (PartPage > 0)
                        PartPage--;

                // Forward button for page
                if (GUI.Button(new Rect(95 * ButtonScale, 100 * ButtonScale, 13 * ButtonScale, 7 * ButtonScale), "", ForwardButtonStyle))
                    PartPage++;

                break;
            case 3: // Settings
                // The usual 3x2 display position stuff
                X1 = 11 * ButtonScale;
                X2 = 71 * ButtonScale;
                X3 = 131 * ButtonScale;
                topY = 43 * ButtonScale;
                bottomY = 70 * ButtonScale;

                // Another 3x2 display. Quite liking how it looks
                
                /*for (int i = 0; i < 6; i++)
                {
                    // The index of the X position to use; 1, 2 or 3
                    int xi = (i > 2) ? i - 3 : i;
                    float ThisX = (xi == 0) ? X1 : ((xi == 1) ? X2 : X3); // if xi is 0, ThisX = X1, otherwise if xi = 1, ThisX = X2, otherwise ThisX = X3

                    // Draw the button
                    GUI.Button(new Rect(ThisX, i > 2 ? bottomY : topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "A Setting!", ButtonStyle);
                }*/

                // Draw the button
                GUI.Box(new Rect(X1, topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Music Volume", SettingBoxStyle);
                SettingBoxStyle.fontSize = ButtonStyle.fontSize;

                float PlusButtonX = Screen.width * 0.078f;
                float MinusButtonX = Screen.width * 0.276f;
                float PlusButtonY = Screen.height * 0.448f;

                // Minus button
                if (GUI.Button(new Rect(MinusButtonX, PlusButtonY, 5 * ButtonScale, 5 * ButtonScale), "", PlusButtonStyle))
                {
                    // Modify volume value in the player data
                    GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume += 0.05f;

                    // Then assign the volume
                    Camera.main.GetComponent<AudioSource>().volume = GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume;
                }

                // Plus button
                if (GUI.Button(new Rect(PlusButtonX, PlusButtonY, 5 * ButtonScale, 5 * ButtonScale), "", MinusButtonStyle))
                {
                    // Modify volume value in the player data
                    GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume -= 0.05f;

                    // Then assign the volume
                    Camera.main.GetComponent<AudioSource>().volume = GameObject.FindObjectOfType<PlayerData>().playerData.MusicVolume;
                }

                break;
        }

        // Draw a back button if we're not on the main (main) menu
        if (MenuID != 0)
            if (GUI.Button(new Rect(ButtonScale, Screen.height - BackButtonImg.height * ButtonScale - ButtonScale, BackButtonImg.width * ButtonScale, BackButtonImg.height * ButtonScale), "", BackButtonStyle))
                MenuID = 0;

        // Draw the amount of gold the player has if ShowGold == true
        if (ShowGold)
        {
            float goldScale = (Screen.width * 0.04f) / GoldIcon.width;
            TextStyle.fontSize = (int)(Screen.height * 0.04f);
            TextStyle.normal.textColor = Color.white;
            int goldAmount = GameObject.FindObjectOfType<PlayerData>().GetGoldAmount();

            GUI.DrawTexture(new Rect(0, 0, GoldIcon.width * goldScale, GoldIcon.height * goldScale), GoldIcon);
            GUI.Label(new Rect(10 + 16 * goldScale, 4 * goldScale, 150, 50), goldAmount.ToString(), TextStyle);
        }
    }

    void EquipItem(int id, PlayerData.SPData playerData)
    {
        // Get a copy of the item info
        ItemInfo itemInfo = GameObject.FindObjectOfType<PartDatabase>().ItemInfoList[id];

        int equipPos = -1;

        if (itemInfo.IsEnchantment)
        {
            // Loop through and find the position to equip it
            // Obviously only 1 enchantment now, but this is designed
            // to work if I expand that one day
            for (int i = 0; i < Basher.MaxEnchantments; i++)
            {
                if (playerData.Enchantments[i] == -1)
                {
                    equipPos = i;
                    break;
                }
            }

            // In theory we'll already have a slot to equip it,
            // but let's just do another check and then equip it
            if (equipPos != -1)
                GameObject.FindObjectOfType<PlayerData>().playerData.Enchantments[equipPos] = id;
        }
        else
        {
            // Loop through and find the position to equip it
            for (int i = 0; i < Basher.MaxParts; i++)
            {
                if (playerData.Parts[i] == -1)
                {
                    equipPos = i;
                    break;
                }
            }

            // In theory we'll already have a slot to equip it,
            // but let's just do another check and then equip it
            if (equipPos != -1)
                GameObject.FindObjectOfType<PlayerData>().playerData.Parts[equipPos] = id;
        }

        // Remember to save progress after each equip/unequip/purchase
        GameObject.FindObjectOfType<PlayerData>().SaveProgress();
    }

    void UnequipItem(int id, PlayerData.SPData playerData)
    {
        // Get a copy of the item info
        ItemInfo itemInfo = GameObject.FindObjectOfType<PartDatabase>().ItemInfoList[id];

        if (itemInfo.IsEnchantment)
        {
            // Loop through and find its position, then uneqip and break from the loop
            for (int i = 0; i < Basher.MaxEnchantments; i++)
            {
                if (playerData.Enchantments[i] == id)
                {
                    // Unequip item - setting to -1 does this
                    GameObject.FindObjectOfType<PlayerData>().playerData.Enchantments[i] = -1;
                    break;
                }
            }
        }
        else
        {
            // Loop through and find its position, then uneqip and break from the loop
            for (int i = 0; i < Basher.MaxParts; i++)
            {
                if (playerData.Parts[i] == id)
                {
                    // Unequip item - setting to -1 does this
                    GameObject.FindObjectOfType<PlayerData>().playerData.Parts[i] = -1;
                    break;
                }
            }
        }
        
        // Remember to save progress after each equip/unequip/purchase
        GameObject.FindObjectOfType<PlayerData>().SaveProgress();
    }

    bool IsSpaceInParts(int id, PlayerData.SPData playerData)
    {
        // Get a copy of the item info
        ItemInfo itemInfo = GameObject.FindObjectOfType<PartDatabase>().ItemInfoList[id];

        bool isSpace = false;

        if (itemInfo.IsEnchantment)
        {
            for (int i = 0; i < Basher.MaxEnchantments; i++)
                if (playerData.Enchantments[i] == -1)
                    isSpace = true;
        }
        else
        {
            for (int i = 0; i < Basher.MaxParts; i++)
                if (playerData.Parts[i] == -1)
                    isSpace = true;
        }

        return isSpace;
    }

    bool IsPartEquipped(int id, PlayerData.SPData playerData)
    {
        bool found = false;

        // Check the parts list
        for (int i = 0; i < playerData.Parts.Length; i++)
            if (playerData.Parts[i] == id)
                found = true;

        // Check the enchantment list
        for (int i = 0; i < playerData.Enchantments.Length; i++)
            if (playerData.Enchantments[i] == id)
                found = true;

        return found;
    }
}
