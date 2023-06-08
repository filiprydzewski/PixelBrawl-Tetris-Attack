using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool onGround;
    public LayerMask groundLayer;
    private Collider2D playerCollider;
    private float moveSpeed = 6f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float fallMultiplier = 2f;
    private float lowJumpMultiplier = 1f;
    private bool canJump = true;
    private float jumpForce = 7f;
    private float rotationSpeed = 10f; // Prêdkoœæ obracania
    private float targetRotation = 0f; // Docelowy k¹t obrotu

    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(GetMovementInput());
        Jump();

        // SprawdŸ, czy gracz jest na ziemi
        onGround = Physics2D.IsTouchingLayers(playerCollider, groundLayer);
        if (onGround)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Tetromino") && collision.otherCollider.CompareTag("Player"))
    //    {
    //        float playerY = collision.otherCollider.transform.position.y;
    //        float tetrominoY = collision.gameObject.transform.position.y;

    //        if (playerY < tetrominoY - 0.001f)
    //        {
    //            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
    //            foreach (AudioSource source in audioSources)
    //            {
    //                source.Stop();
    //            }
    //            Time.timeScale = 0f;
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tetromino") && collision.otherCollider.CompareTag("Player"))
        {
            float playerY = collision.otherCollider.transform.position.y;
            float tetrominoY = collision.gameObject.transform.position.y;
            float playerX = collision.otherCollider.transform.position.x;
            float tetrominoX = collision.gameObject.transform.position.x;

            Collider2D playerCollider = collision.otherCollider;
            Collider2D tetrominoCollider = collision.collider;

            if (playerY < tetrominoY - 0.001f && playerCollider.bounds.Intersects(tetrominoCollider.bounds))
            {
                AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                foreach (AudioSource source in audioSources)
                {
                    source.Stop();
                }
                Time.timeScale = 0f;
            }
        }
    }



    private float GetMovementInput()
    {
        if (Input.GetKey(moveLeftKey))
        {
            return -1f;
        }
        else if (Input.GetKey(moveRightKey))
        {
            return 1f;
        }
        else
        {
            return 0f;
        }
    }

    private void Move(float direction)
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Obliczanie docelowego k¹ta obrotu
        if (direction > 0)
        {
            targetRotation = 180f; // Kierunek w prawo (0 stopni)
        }
        else if (direction < 0)
        {
            targetRotation = 0f; // Kierunek w lewo (180 stopni)
        }

        // P³ynne obracanie postaci
        float currentRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotation, transform.eulerAngles.z);
    }

    private void Jump()
    {
        // Skakanie
        if (canJump && Input.GetKeyDown(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Modyfikacja opadania
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }
}
