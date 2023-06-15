using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject cameraTrigger;
    public GameObject tetrisManager;
    public string tetrominoTag = "Tetromino";

    private float cameraSpeed = 2f;
    private bool isMovingCamera;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;
    private Vector3 initialBackgroundPosition;


    public Transform bg1;
    public Transform bg2;

    private float size;

    private void Start()
    {
        initialCameraPosition = cameraTransform.position;

        size = bg1.GetComponent<BoxCollider2D>().size.y;
    }

    private void Update()
    {
        if (IsTetrominoTouchingTrigger() && !IsTouchingTetrominoMoving())
        {
            if (!isMovingCamera)
            {
                isMovingCamera = true;
                targetCameraPosition = cameraTransform.position + Vector3.up * cameraTrigger.transform.localScale.y;
            }
        }

        if (isMovingCamera)
        {
            MoveCamera();
            MoveCameraTrigger();
            MoveTetrisManager();
        }

        if(cameraTransform.position.y >= bg2.position.y) {
            bg1.position = new Vector3(bg1.position.x, bg2.position.y+size, bg1.position.z);
            SwitchBG();
        }
        
    }


    private void SwitchBG() {
        Transform temp =bg1;
        bg1 = bg2;
        bg2 = temp;
    }


    private bool IsTetrominoTouchingTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(cameraTrigger.transform.position, cameraTrigger.transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tetrominoTag))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTouchingTetrominoMoving()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(cameraTrigger.transform.position, cameraTrigger.transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tetrominoTag))
            {
                Rigidbody2D tetrominoRigidbody = collider.GetComponent<Rigidbody2D>();
                if (tetrominoRigidbody != null && !tetrominoRigidbody.IsSleeping())
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MoveCamera()
    {
        Vector3 targetPosition = cameraTransform.position + Vector3.up * cameraSpeed * Time.deltaTime;
        cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, targetPosition, cameraSpeed * Time.deltaTime);

        if (Vector3.Distance(cameraTransform.position, initialCameraPosition) >= 0.5f)
        {
            isMovingCamera = false;
        }
    }

    private void MoveCameraTrigger()
    {
        Vector3 triggerPosition = cameraTrigger.transform.position + Vector3.up * cameraSpeed * Time.deltaTime;
        cameraTrigger.transform.position = triggerPosition;
    }

    private void MoveTetrisManager()
    {
        Vector3 tetrisManagerPosition = tetrisManager.transform.position + Vector3.up * cameraSpeed * Time.deltaTime;
        tetrisManager.transform.position = tetrisManagerPosition;
    }

}