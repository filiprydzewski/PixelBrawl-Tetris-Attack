using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool onGround;
    private float maxStamina = 100f;
    private float currentStamina;
    private float staminaRegenSpeed = 10f;
    private bool canDoubleJump = false;
    public LayerMask groundLayer;
    private Collider2D playerCollider;
    private float moveSpeed = 6f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float fallMultiplier = 2f;
    private float lowJumpMultiplier = 5f;
    private bool canJump = true;
    private float jumpForce = 11f;
    private float rotationSpeed = 10f; // Pr�dko�� obracania
    private float targetRotation = 0f; // Docelowy k�t obrotu

    public AudioSource soundEffect;
    private enum MovementState { idle, walking, jumping, doubleJump };
    private Animator anim;
    MovementState state;

    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float staminaX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        state = MovementState.idle;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<ObjectStunning>().isStunned)
        {
            Move(GetMovementInput());
            Jump();

            if (currentStamina < 100f)
            {
                currentStamina += staminaRegenSpeed * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tetromino") && collision.otherCollider.CompareTag("Player"))
        {
            float playerY = collision.otherCollider.transform.position.y;
            float tetrominoY = collision.gameObject.transform.position.y;

            Collider2D playerCollider = collision.otherCollider;
            Collider2D tetrominoCollider = collision.collider;

            if (playerY < tetrominoY - 0.001f && playerCollider.bounds.Intersects(tetrominoCollider.bounds))
            {
                AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                foreach (AudioSource source in audioSources)
                {
                    soundEffect.Play();
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

        // Obliczanie docelowego k�ta obrotu
        if (direction > 0)
        {
            state = MovementState.walking;
            targetRotation = 180f; // Kierunek w prawo (0 stopni)
        }
        else if (direction < 0)
        {
            state = MovementState.walking;
            targetRotation = 0f; // Kierunek w lewo (180 stopni)
        }
        else
        {
            state = MovementState.idle;
        }

        // P�ynne obracanie postaci
        float currentRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotation, transform.eulerAngles.z);
    }

    private void Jump()
    {
        //GroundCheck
        onGround = Physics2D.IsTouchingLayers(playerCollider, groundLayer);
        if (onGround)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
            state = MovementState.jumping;
        }

        // Skakanie
        if (canJump && Input.GetKeyDown(jumpKey))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = true;
        }
        else if (canDoubleJump && Input.GetKeyDown(jumpKey) && currentStamina >= 30f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = false;
            currentStamina -= 30f;
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

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 60;
        GUI.Box(new Rect(staminaX, 0, 500, 100), "Stamina: " + (int)currentStamina, style);
    }
}