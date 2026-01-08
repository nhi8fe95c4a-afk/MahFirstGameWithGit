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
    private const string AutoGCName = "_Auto_GroundCheck";

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

        // Проверка земли (без падения, если groundCheck вдруг не задан)
        bool grounded = false;
        if (groundCheck != null && jumpCooldown <= 0f) // Не проверяем землю сразу после прыжка
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        }
        groundedDebug = grounded;
        if (grounded)
        {
            lastOnGroundTime = coyoteTime;
            jumpConsumed = false;
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
        return lastOnGroundTime > 0f && lastJumpPressedTime > 0f && !jumpConsumed;
    }

    private void DoJump()
    {
        jumpConsumed = true;
        lastOnGroundTime = 0f;
        lastJumpPressedTime = 0f;
        jumpCooldown = 0.15f; // Задержка перед следующей проверкой земли

        Vector2 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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

