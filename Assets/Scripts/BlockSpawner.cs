using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;   // Assign building block prefab
    public Transform spawnPoint;     // Position where blocks spawn
    public float dropHeight = 5f;    // How high above to spawn

    private GameObject currentBlock;

    void Update()
    {
        if (currentBlock == null)
        {
            SpawnBlock();
        }

        if (Input.GetMouseButtonDown(0)) // Mouse click / tap
        {
            DropBlock();
        }
    }

    void SpawnBlock()
    {
        Vector3 pos = new Vector3(0, Camera.main.transform.position.y + dropHeight, 0);
        currentBlock = Instantiate(blockPrefab, pos, Quaternion.identity);
        currentBlock.GetComponent<Rigidbody2D>().gravityScale = 0; // no fall yet
    }

    void DropBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.GetComponent<Rigidbody2D>().gravityScale = 1; // enable falling
            currentBlock = null;
        }
    } 
}
