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
       //Block.groundTransform = groundTransform;
        SpawnBlock();
    }

    void Update()
    {
        if (currentBlock != null)
        {
            
            float horizontalMovement = moveDirection * moveSpeed * Time.deltaTime;
            currentBlock.transform.position += new Vector3(horizontalMovement, 0, 0);

            
            if (currentBlock.transform.position.x > 3f || currentBlock.transform.position.x < -3f)
            {
                moveDirection *= -1;
            }
        }
        else
        {
            return;
        }

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

        if (topBlock != null && currentBlock != null)
        {
            
            Vector3 mid = (topBlock.transform.position + currentBlock.transform.position) / 2f;
            

            cameraTarget.position = Vector3.Lerp(
                cameraTarget.position,
                new Vector3(0, mid.y, 0),
                Time.deltaTime * 3f 
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