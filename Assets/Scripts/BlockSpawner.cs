using System.Collections;
using UnityEngine;
using Cinemachine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float dropHeight = 5f;
    public float moveSpeed = 5f;
    public CinemachineVirtualCamera vCam;
    public Transform cameraTarget;

    // New variable to control the camera's zoom level
    public float cameraOrthographicSize = 6f;

    private GameObject currentBlock;
    private GameObject topBlock;
    private bool isMovingRight = true;
    private float moveDirection = 1f;

    private void Start()
    {
        SpawnBlock();
    }

    void Update()
    {
        if (currentBlock != null)
        {
            // Move the current block back and forth
            float horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
            currentBlock.transform.position += new Vector3(horizontalMovement, 0, 0);

            // Reverse direction if it hits a boundary (adjust as needed)
            if (currentBlock.transform.position.x > 5f || currentBlock.transform.position.x < -5f)
            {
                moveDirection *= -1;
            }
        }
        else
        {
            return;
        }

        // Drop the block on left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            DropBlock();
        }

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

            StartCoroutine(CheckBlockLanded(currentBlock));
            currentBlock = null;
        }
    }

    IEnumerator CheckBlockLanded(GameObject block)
    {
        Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
        yield return new WaitUntil(() => rb.velocity.sqrMagnitude < 0.1f);

        topBlock = block;
        yield return new WaitForSeconds(0.5f);

        SpawnBlock();
    }

    void UpdateCameraTarget()
    {
        if (cameraTarget == null || vCam == null) return;

        // **PRIMARY CHANGE:** The camera now follows the currently active block.
        if (topBlock != null && currentBlock != null)
        {
            // Midpoint between top block and current block
            Vector3 mid = (topBlock.transform.position + currentBlock.transform.position) / 2f;
            // cameraTarget.position = new Vector3(0, mid.y, 0);

            cameraTarget.position = Vector3.Lerp(
                cameraTarget.position,
                new Vector3(0, mid.y, 0),
                Time.deltaTime * 3f // adjust speed
            );

        }
        /*else if (topBlock != null)
        {
            // Fallback: If no block is currently moving, follow the top block of the tower.
            Vector3 targetPos = new Vector3(0, topBlock.transform.position.y, 0);
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos, Time.deltaTime * 5f);
        }*/

        // **NEW:** Set the camera's zoom level
        vCam.m_Lens.OrthographicSize = cameraOrthographicSize;
    }
}