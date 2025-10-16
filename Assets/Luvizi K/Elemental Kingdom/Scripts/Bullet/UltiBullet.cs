using UnityEngine;

public class UltiBullet : BulletBase
{
    [SerializeField] private float knockbackForce = 10f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == shooter) return;

        // Gây damage nhờ BulletBase
        base.OnTriggerEnter2D(collision);

        // Knockback thêm cho Enemy
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
    }
}
