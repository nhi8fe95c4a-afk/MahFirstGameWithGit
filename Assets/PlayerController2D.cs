using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 60f;
    public float deceleration = 70f;

    [Header("Jump")]
    public float jumpForce = 13f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float lowJumpMultiplier = 2f;
    public float fallMultiplier = 2.5f;
    public int maxJumps = 2; // 1 = single jump, 2 = double jump, etc.
    public float airJumpMultiplier = 0.9f; // Air jumps are slightly weaker

    [Header("Ground Check")]
    public Transform groundCheck;          // можно не назначать: создадим автоматически
    public float groundCheckRadius = 0.15f;
    public LayerMask groundMask;

    [Header("Debug")]
    [SerializeField] private bool groundedDebug;

    private Rigidbody2D rb;
    private Collider2D col;

    private float inputX;
    private bool jumpHeld;

    private float lastOnGroundTime;
    private float lastJumpPressedTime;

    private bool jumpConsumed;
    private float jumpCooldown; // Задержка после прыжка для предотвращения застревания
    private int jumpsRemaining; // Количество оставшихся прыжков
    private const string AutoGCName = "_Auto_GroundCheck";
    private const float JUMP_GROUND_CHECK_COOLDOWN = 0.15f; // Время до повторной проверки земли после прыжка

    void Reset()
    {
        // Если скрипт только что добавили — попытаемся создать GroundCheck сразу
        TryEnsureGroundCheck();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Предотвращает проваливание

        TryEnsureGroundCheck();
    }

    void Update()
    {
        // Ввод
        inputX = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = jumpBufferTime;
        jumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.Space);

        // Таймеры
        lastOnGroundTime -= Time.deltaTime;
        lastJumpPressedTime -= Time.deltaTime;
        jumpCooldown -= Time.deltaTime;

        // Проверка земли
        bool grounded = ShouldCheckGround() && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        groundedDebug = grounded;
        if (grounded)
        {
            lastOnGroundTime = coyoteTime;
            jumpConsumed = false;
            jumpsRemaining = maxJumps; // Восстанавливаем все прыжки при приземлении
        }

        // Прыжок
        if (CanJump())
            DoJump();

        // «Лучший» прыжок
        ApplyBetterJump();
    }

    void FixedUpdate()
    {
        float target = inputX * moveSpeed;
        float accel = Mathf.Abs(target) > 0.01f ? acceleration : deceleration;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, target, accel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    private bool CanJump()
    {
        // Прыжок с земли (включая coyote time и jump buffer)
        bool groundJump = lastOnGroundTime > 0f && lastJumpPressedTime > 0f && !jumpConsumed;
        
        // Воздушный прыжок (double jump, triple jump, etc.)
        bool airJump = lastJumpPressedTime > 0f && jumpsRemaining > 0 && lastOnGroundTime <= 0f;
        
        return groundJump || airJump;
    }

    private bool ShouldCheckGround()
    {
        // Не проверяем землю если:
        // - groundCheck не настроен
        // - мы только что прыгнули (cooldown активен)
        return groundCheck != null && jumpCooldown <= 0f;
    }

    private void DoJump()
    {
        // Определяем, это прыжок с земли или воздушный прыжок
        bool isGroundJump = lastOnGroundTime > 0f;
        
        jumpConsumed = true;
        lastOnGroundTime = 0f;
        lastJumpPressedTime = 0f;
        
        if (isGroundJump)
        {
            jumpCooldown = JUMP_GROUND_CHECK_COOLDOWN;
            jumpsRemaining = maxJumps - 1; // Используем один прыжок
        }
        else
        {
            // Воздушный прыжок
            jumpsRemaining--; // Уменьшаем количество оставшихся прыжков
        }

        Vector2 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        // Воздушные прыжки немного слабее
        float jumpPower = isGroundJump ? jumpForce : jumpForce * airJumpMultiplier;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void ApplyBetterJump()
    {
        if (rb.linearVelocity.y < -0.01f)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime);
        }
        else if (rb.linearVelocity.y > 0.01f && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime);
        }
    }

    private void TryEnsureGroundCheck()
    {
        if (groundCheck != null) return;

        // Попробуем найти среди детей объект по имени
        var t = transform.Find(AutoGCName);
        if (t == null)
        {
            var go = new GameObject(AutoGCName);
            t = go.transform;
            t.SetParent(transform);
            t.localPosition = new Vector3(0f, -0.6f, 0f); // чуть ниже "ног"
        }
        groundCheck = t;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = groundedDebug ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

