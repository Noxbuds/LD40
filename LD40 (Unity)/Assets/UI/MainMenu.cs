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

    // UI Elemeents
    public Texture2D GoldIcon;
    public Texture2D Background;
    public Texture2D ButtonImg;
    public Texture2D BackButtonImg;
    public Font GameFont;
    public GUIStyle TextStyle;
    public GUIStyle ButtonStyle; // create in editor
    public GUIStyle BackButtonStyle; // create in editor

	// Don't need to load at startup; the player data handler already does this
    void Start()
    {
        TextStyle = new GUIStyle();
        TextStyle.font = GameFont;
    }

    // GUI Code (Yes the legacy GUI system is slower and can be CPU intense,
    // but I've never taken the time to learn the 'new' one...)
    void OnGUI()
    {
        // Scale for the buttons
        float ButtonScale = (Screen.width * 0.276f) / ButtonImg.width;
        ButtonStyle.fontSize = (int)(Screen.height * 0.04f);

        // Draw menu elements
        switch (MenuID)
        {
            case 0: // Main Menu
                ShowGold = false;

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
                    MenuID = 2;

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
                            SceneManager.LoadScene("Level " + (i + 1).ToString());
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

                for (int i = 0; i < 6; i++)
                {
                    // The index of the X position to use; 1, 2 or 3
                    int xi = (i > 2) ? i - 3 : i;
                    float ThisX = (xi == 0) ? X1 : ((xi == 1) ? X2 : X3); // if xi is 0, ThisX = X1, otherwise if xi = 1, ThisX = X2, otherwise ThisX = X3

                    // Draw the button
                    GUI.Button(new Rect(ThisX, i > 2 ? bottomY : topY, ButtonImg.width * ButtonScale, ButtonImg.height * ButtonScale), "Under construction!", ButtonStyle);
                }

                break;
            case 3: // Options
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
}
