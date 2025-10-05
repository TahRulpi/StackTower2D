using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float dropHeight = 5f;
    public float moveSpeed = 5f;
    public CinemachineVirtualCamera vCam;
    public Transform cameraTarget;
    public float cameraOrthographicSize = 6f;
    public Transform groundTransform;

    private GameObject currentBlock;
    private GameObject topBlock;
    private bool isMovingRight = true;
    private float moveDirection = 0.5f;

    private void Start()
    {
        // Block.groundTransform = groundTransform; // Assuming this line is for a variable not shown
        SpawnBlock();
    }

    void Update()
    {
        // Handle input only if a block is currently moving
        if (currentBlock != null)
        {
            // 1. Block Movement
            float horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
            currentBlock.transform.position += new Vector3(horizontalMovement, 0, 0);

            // 2. Edge Detection (Reverse direction)
            if (currentBlock.transform.position.x > 3f || currentBlock.transform.position.x < -3f)
            {
                moveDirection *= -1;
            }

            // 3. Drop Input
            if (Input.GetMouseButtonDown(0))
            {
                DropBlock();
            }
        }
        // If currentBlock is null, we are waiting for the previous block to land or the game is over.

        UpdateCameraTarget();
    }

    void SpawnBlock()
    {
        float spawnY = (topBlock != null ? topBlock.transform.position.y : 0f) + dropHeight;
        Vector3 pos = new Vector3(0, spawnY, 0);

        currentBlock = Instantiate(blockPrefab, pos, Quaternion.identity);
        currentBlock.GetComponent<Rigidbody2D>().gravityScale = 0;
        moveDirection = isMovingRight ? 1f : -1f;
        isMovingRight = !isMovingRight;
    }

    void DropBlock()
    {
        if (currentBlock != null)
        {
            Rigidbody2D rb = currentBlock.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;

            // Start coroutine to check landing and then spawn the next block
            StartCoroutine(CheckBlockLanded(currentBlock));

            // CRITICAL: Set currentBlock to null immediately so input is ignored while it drops
            currentBlock = null;
        }
    }

    IEnumerator CheckBlockLanded(GameObject block)
    {
        Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
        // Wait until velocity is near zero (meaning it has landed)
        yield return new WaitUntil(() => rb.velocity.sqrMagnitude < 0.1f);

        // Update the top block of the stack
        topBlock = block;

        // Wait briefly, then spawn the next block, only if the game hasn't ended
        if (Time.timeScale > 0) // Check if the game is not paused (i.e., not game over)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnBlock();
        }
    }

    void UpdateCameraTarget()
    {
        if (cameraTarget == null || vCam == null) return;

        if (topBlock != null && currentBlock != null)
        {
            Vector3 mid = (topBlock.transform.position + currentBlock.transform.position) / 2f;

            cameraTarget.position = Vector3.Lerp(
                cameraTarget.position,
                new Vector3(0, mid.y, 0),
                Time.deltaTime * 3f
            );
        }

        vCam.m_Lens.OrthographicSize = cameraOrthographicSize;
    }


}
