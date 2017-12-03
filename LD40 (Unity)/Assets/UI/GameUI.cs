using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    // Draws the game's HUD/UI
    // Using the pre-4.5 Unity GUI system because I don't
    // know how to use the 'new' one and I don't want to
    // spend unnecessary time learning it :P

    // Local references
    public Damagable BasherDmgb;
    public Damagable WallDmgb;

    // Images
    public Texture2D HealthArtFG;
    public Texture2D HealthArtBG;
    public Texture2D Plain; // 8x8 plain white image
    public Texture2D SuccessImg;
    public Texture2D FailureImg;

    private bool WonGame;
    private bool GameEnded;

    // I wonder if/when they're going to remove this old
    // system...
	void OnGUI()
    {
        // Game has been won
        if (WonGame)
        {
            float scale = Screen.width / SuccessImg.width;
            GUI.DrawTexture(new Rect(Screen.width / 2 - SuccessImg.width / 2 * scale, Screen.height / 2 - SuccessImg.height / 2 * scale, SuccessImg.width * scale, SuccessImg.height * scale), SuccessImg);
        }

        // Game has been lost
        if (!WonGame && GameEnded)
        {
            float scale = Screen.width / FailureImg.width;
            GUI.DrawTexture(new Rect(Screen.width / 2 - FailureImg.width / 2 * scale, Screen.height / 2 - FailureImg.height / 2 * scale, FailureImg.width * scale, FailureImg.height * scale), FailureImg);
        }

        // Normal UI code
        if (!WonGame && !GameEnded)
        {
            // Scale the image up correctly to fit the screen nicely
            float scale = Screen.width / HealthArtBG.width;
            float ypos = Screen.height - HealthArtBG.height * scale;

            // Since we don't want to stretch on the X-axis, let's centre it instead
            float xpos = Screen.width / 2 - (HealthArtBG.width * scale) / 2;

            // Draw the background first
            GUI.DrawTexture(new Rect(xpos, ypos, HealthArtBG.width * scale, HealthArtBG.height * scale), HealthArtBG);

            // Then draw the player health bar
            float PlrPercentage = BasherDmgb.Health / 100.0f;
            if (PlrPercentage < 0)
                PlrPercentage = 0;

            float PlrXPos = Screen.width / 2 - HealthArtFG.width * 0.5f * scale * PlrPercentage;
            GUI.DrawTexture(new Rect(PlrXPos, ypos + HealthArtFG.height * 0.5f * scale - 0.5f * scale, HealthArtFG.width * 0.5f * scale * PlrPercentage, HealthArtFG.height * scale * 0.5f), PlainTex(new Color(0.2f, 1.0f, 0.2f)));

            // Then the wall health bar
            float WallPercentage = GameObject.FindObjectOfType<Castle>().TotalHealth / GameObject.FindObjectOfType<Castle>().MaxHealth;
            if (WallPercentage < 0)
                WallPercentage = 0;

            float WallXPos = Screen.width / 2;
            GUI.DrawTexture(new Rect(WallXPos, ypos + HealthArtFG.height * 0.5f * scale - 0.5f * scale, HealthArtFG.width * 0.5f * scale * WallPercentage, HealthArtFG.height * scale * 0.5f), PlainTex(new Color(1.0f, 0.2f, 0.2f)));

            // And finally draw the beautiful castle art on top
            GUI.DrawTexture(new Rect(xpos, ypos, HealthArtFG.width * scale, HealthArtFG.height * scale), HealthArtFG);

        }
    }

    // Switch to the win screen
    public void WinGame()
    {
        WonGame = true;
        GameEnded = true;
    }

    // Switch to the lose screen
    public void LoseGame()
    {
        WonGame = false;
        GameEnded = true;
    }

    // Create an 8x8 plain colour image
    Texture2D PlainTex(Color colour)
    {
        Texture2D newImg = new Texture2D(8, 8);
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                newImg.SetPixel(x, y, colour);

        newImg.Apply();
        return newImg;
    }
}
