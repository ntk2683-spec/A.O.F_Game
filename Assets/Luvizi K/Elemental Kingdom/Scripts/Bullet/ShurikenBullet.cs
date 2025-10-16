using UnityEngine;

public class ShurikenBullet : BulletBase
{
    // Nếu muốn, có thể override Init để set các thông số riêng cho Shuriken
    // hoặc thêm hiệu ứng đặc biệt khi va chạm.

    // Ví dụ, thêm hiệu ứng khi va chạm
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // vẫn gây damage cơ bản

        // Thêm các hiệu ứng khác nếu muốn
        // Ví dụ: particle effect, âm thanh...
    }
}
