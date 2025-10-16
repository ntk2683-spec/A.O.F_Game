using UnityEngine;

public class UltimateSkill : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePos;
    [SerializeField] private int bulletCount = 12;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldown = 5f;   // thời gian hồi chiêu (giây)
    private float nextCastTime = 0f;                // thời điểm có thể dùng lại

    public void ActivateUltimate()
    {
        if (Time.time < nextCastTime)
        {
            Debug.Log("Ultimate chưa hồi xong!");
            return; // chưa đủ thời gian hồi chiêu
        }

        // bắn ulti
        CastUltimate();

        // set lại thời gian hồi
        nextCastTime = Time.time + cooldown;
    }

    private void CastUltimate()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 dir = new Vector2(dirX, dirY).normalized;

            Quaternion rot = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, rot);

            // Lấy BulletBase và khởi tạo
            BulletBase bulletScript = bullet.GetComponent<BulletBase>();
            if (bulletScript != null)
            {
                // dmg bạn có thể chỉnh = ATK của Player hoặc giá trị cố định
                int ultiDamage = 50;
                bulletScript.Init(dir, ultiDamage, gameObject);
            }

            angle += angleStep;
        }
    }

    // Hàm này có thể gọi từ UI để hiển thị % hồi chiêu
    public float GetCooldownRemaining()
    {
        return Mathf.Max(0f, nextCastTime - Time.time);
    }
}
