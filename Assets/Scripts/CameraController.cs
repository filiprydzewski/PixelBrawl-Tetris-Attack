using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform targetElement;
    public GameObject cameraTrigger;
    public GameObject tetrisManager;
    public string tetrominoTag = "Tetromino";

    private float cameraSpeed = 5f;
    private bool isMovingCamera;
    private Vector3 initialCameraPosition;
    private Vector3 targetCameraPosition;
    private Vector3 initialBackgroundPosition;
    public Transform background;


    private void Start()
    {
        initialCameraPosition = cameraTransform.position;
        initialBackgroundPosition = background.position;
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
            MoveTargetElement();
            MoveCameraTrigger();
            MoveTetrisManager();
            MoveBackground();
        }
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
                if (tetrominoRigidbody != null && tetrominoRigidbody.velocity.y < 0.000001)
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

    private void MoveTargetElement()
    {
        Vector3 targetElementPosition = targetElement.position + Vector3.up * cameraSpeed * Time.deltaTime;
        targetElement.position = targetElementPosition;
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

    private void MoveBackground()
    {
        Vector3 backgroundPosition = initialBackgroundPosition + (cameraTransform.position - initialCameraPosition);
        background.position = backgroundPosition;
    }
}