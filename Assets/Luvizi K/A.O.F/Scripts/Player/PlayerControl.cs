using System.Collections;
using UnityEngine;
public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;        // tốc độ di chuyển bình thường
    [SerializeField] private float dashSpeed = 12f;       // tốc độ dash
    [SerializeField] private float dashDuration = 0.2f;   // thời gian dash (giây)
    public Joystick joystick;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public static PlayerControl Instance;
    [SerializeField] private GameObject shield;
    private bool isDashing = false;
    private bool isAttacking = false; // cờ trạng thái tấn công
    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        shield.SetActive(false);

    }
    private void FixedUpdate()
    {
        if (isDashing) return; // Khi đang dash thì không xử lý move thường
        // Lấy input từ joystick
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;
        movement = new Vector2(moveHorizontal, moveVertical).normalized;
        // Di chuyển
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        // 👉 Nếu không tấn công thì quay mặt theo joystick
        if (!isAttacking && movement != Vector2.zero)
        {
            FaceDirection(movement);
        }
        // Animation chạy
        animator.SetBool("isRun", movement != Vector2.zero);
    }
    // Gọi hàm này bằng Button UI
    public void DashButton()
    {
        if (!isDashing)

            StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        // Hướng dash theo joystick hoặc hướng đang facing
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
        // Toggle shield
        shield.SetActive(!shield.activeSelf);
    }
    // 🔥 Gọi khi vũ khí bắn → vừa attack vừa quay mặt theo enemy/hướng bắn
    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        // tự động reset trạng thái sau animation
        StartCoroutine(ResetAttackFlag());
    }
    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.3f); // tùy theo thời gian animation
        isAttacking = false;
    }
    // Hàm xoay mặt nhân vật theo hướng bất kỳ
    public void FaceDirection(Vector2 dir)
    {
        if (dir.x < 0)
            spriteRenderer.flipX = true;
        else if (dir.x > 0)
            spriteRenderer.flipX = false;
    }
    // Hàm xoay mặt theo vị trí target (enemy)
    public void FaceTarget(Vector3 targetPos)
    {
        Vector2 dir = targetPos - transform.position;
        FaceDirection(dir);
    }
}