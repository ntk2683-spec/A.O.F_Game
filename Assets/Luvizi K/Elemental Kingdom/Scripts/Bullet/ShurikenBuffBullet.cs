using UnityEngine;

public class ShurikenBuffBullet : BulletBase
{
    [SerializeField] private float knockbackForce = 10f; // lực đẩy lùi

    /// <summary>
    /// Nếu muốn, có thể override Init để set riêng knockback hoặc moveSpeed
    /// </summary>
    public override void Init(Vector2 dir, int dmg, GameObject shooterObj)
    {
        base.Init(dir, dmg, shooterObj);
        // có thể set moveSpeed hoặc knockbackForce riêng nếu muốn
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Không đánh trúng chính shooter
        if (collision.gameObject == shooter) return;

        // Gọi cơ chế damage cơ bản từ BulletBase
        base.OnTriggerEnter2D(collision);

        // Thêm knockback nếu va chạm với EnemyBase
        EnemyBase enemy = collision.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDir = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }

        // Destroy bullet sau khi va chạm (nếu chưa destroy bởi base)
        Destroy(gameObject);
    }
}
