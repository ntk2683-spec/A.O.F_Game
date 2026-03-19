using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerBase playerBase; // Khai báo để lấy stats
    
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    
    public Joystick joystick;
    private Vector2 movement;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public static PlayerControl Instance;

    [SerializeField] private GameObject shield;

    private bool isDashing = false;
    private bool isAttacking = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Lấy script PlayerBase nằm cùng trên GameObject này
        playerBase = GetComponent<PlayerBase>();

        if (shield != null) shield.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        // SỬ DỤNG TRỰC TIẾP playerBase.SPD TẠI ĐÂY
        float currentSpeed = (playerBase != null) ? playerBase.SPD : 3f;
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        if (!isAttacking && movement != Vector2.zero)
        {
            FaceDirection(movement);
        }

        animator.SetBool("isRun", movement != Vector2.zero);
    }

    public void DashButton()
    {
        if (!isDashing) StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        if (dashDirection == Vector2.zero)
        {
            dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection.normalized * dashSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;
    }

    public void Shield()
    {
        if (shield != null) shield.SetActive(!shield.activeSelf);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(ResetAttackFlag());
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.3f); 
        isAttacking = false;
    }

    public void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0) spriteRenderer.flipX = true;
        else if (dir.x > 0) spriteRenderer.flipX = false;
    }

    public void FaceTarget(Vector3 targetPos)
    {
        Vector2 dir = targetPos - transform.position;
        FaceDirection(dir);
    }
}