using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    // A flag to indicate if this is the first block
    public static bool isFirstBlock = true;

    // The ground transform, used to check if the block is on the ground
    public static Transform groundTransform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isFirstBlock)
            {
                isFirstBlock = false;
                Debug.Log("First block landed.");
                return; 
            }

            if (transform.position.y <= groundTransform.position.y + 0.5f)
            {
                Debug.Log("Game Over! A subsequent block touched the ground.");
                SceneManager.LoadScene("Menu");
            }
        }
    }
}