using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // The first block (the one resting on the ground) should not trigger Game Over.
    // This is static because we need to track it across all block instances.
    public static bool isFirstBlock = true;

    // IMPORTANT: Reference to the Game Over Panel. This MUST be set in the Inspector 
    // on your block prefab.
    public GameObject gameOverPanel;

    public void Start()
    {
       


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with the ground layer/tag
        if (collision.gameObject.CompareTag("Ground"))
        {
            // If it's the very first block, set the flag and exit.
            if (isFirstBlock)
            {
                isFirstBlock = false;
                Debug.Log("First block landed successfully.");
                return;
            }

            // If any subsequent block hits the ground, it's Game Over.
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        // Prevent multiple game over triggers
        if (Time.timeScale == 0f)
        {
            return;
        }

        Debug.Log("Game Over Triggered! Block touched the ground.");


        // 1. Activate the Game Over Panel
        // This relies on you setting the 'gameOverPanel' public variable on the prefab.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            // Fallback: Try to find the panel if the Inspector link was missed.
            GameObject panel = GameObject.Find("GameOverPanel"); // Adjust this name if your panel is named differently
            if (panel != null)
            {
                panel.SetActive(true);
            }
            else
            {
                Debug.LogError("Game Over Panel is NULL! Make sure it is assigned in the Block Prefab Inspector OR named 'GameOverPanel' in the scene.");
            }
        }


        // 2. Deactivate the Gameplay UI
        GameObject gameplayUI = GameObject.Find("GameplayUI");
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        // 3. Stop the Block Spawner
        BlockSpawner spawner = FindObjectOfType<BlockSpawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }

        // 4. Stop the game completely (MUST be the LAST step to ensure UI updates finish)
        Time.timeScale = 0f;
    }
}
