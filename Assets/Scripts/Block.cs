using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static bool isFirstBlock = true;

    // Reference to the Game Over Panel
    public GameObject gameOverPanel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isFirstBlock)
            {
                isFirstBlock = false;
                Debug.Log("First block landed.");
                return;
            }

            // **Trigger Game Over immediately when hitting ground**
            Debug.Log("Game Over! Block touched the ground.");
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            //also set active false the game play ui
            GameObject gameplayUI = GameObject.Find("GameplayUI");
            if (gameplayUI != null)
            {
                gameplayUI.SetActive(false);
            }



            // Stop the game (optional)
            Time.timeScale = 0f;
        }
    }
}
