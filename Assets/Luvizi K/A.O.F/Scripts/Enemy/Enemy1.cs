using UnityEngine;

public class Enemy1 : EnemyBase
{
    [SerializeField] private float attackRange = 1.2f;

    // Enemy1 sẽ có cách tấn công riêng
    public override void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            // Kiểm tra khoảng cách với Player
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                nextAttackTime = Time.time + attackCooldown;
                // Ví dụ: gây sát thương cho Player (nếu Player có hàm TakeDamage)
                // player.TakeDamage(attackPower);
            }
        }
    }

    // Nếu muốn Enemy1 vừa di chuyển vừa tấn công
    protected override void Update()
    {
        base.Update();   // vẫn giữ logic di chuyển của EnemyBase
        Attack();        // thêm hành vi tấn công riêng
    }
}
