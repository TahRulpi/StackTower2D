using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Ground"))
        {
            // Successfully stacked
           // GetComponent<Rigidbody2D>().gravityScale = 0; // stop falling
            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Update()
    {
        if (transform.position.y < -10f) // Fell off screen
        {
            Destroy(gameObject);
            Debug.Log("Game Over!");
        }
    }
}
