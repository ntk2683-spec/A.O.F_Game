using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int maxMP = 50;
    [SerializeField] private int attack = 10;
    [SerializeField] private int defense = 5;
    [SerializeField] private float speed = 3f;

    public int CurrentHP { get; private set; }
    public int CurrentMP { get; private set; }
    public int ATK => attack;
    public int DEF => defense;
    public float SPD => speed;

    protected virtual void Awake()
    {
        CurrentHP = maxHP;
        CurrentMP = maxMP;
    }

    // 🔹 Hàm nhận sát thương
    public virtual void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - DEF, 1);
        CurrentHP -= finalDamage;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        Debug.Log($"{gameObject.name} nhận {finalDamage} damage! HP còn: {CurrentHP}");

        if (CurrentHP <= 0)
        {
            Die();
        }
    }
    public void FaceTarget(Vector3 targetPos)
    {
        Vector3 scale = transform.localScale;
        if (targetPos.x < transform.position.x)
            scale.x = -Mathf.Abs(scale.x); // quay trái
        else
            scale.x = Mathf.Abs(scale.x); // quay phải

        transform.localScale = scale;

        Debug.Log("FaceTarget called, scale.x = " + scale.x);
    }


    // 🔹 Hàm hồi HP
    public virtual void Heal(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, maxHP);
        Debug.Log($"{gameObject.name} hồi {amount} HP! HP hiện tại: {CurrentHP}");
    }

    // 🔹 Hàm hồi MP
    public virtual void RestoreMP(int amount)
    {
        CurrentMP += amount;
        CurrentMP = Mathf.Min(CurrentMP, maxMP);
        Debug.Log($"{gameObject.name} hồi {amount} MP! MP hiện tại: {CurrentMP}");
    }

    // 🔹 Khi nhân vật chết
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} đã chết!");
        // Thêm logic khi Player chết: respawn, game over, v.v.
    }

    // 🔹 Hàm tăng chỉ số (ví dụ nâng cấp)
    public void AddStat(string stat, int amount)
    {
        switch (stat.ToUpper())
        {
            case "HP":
                maxHP += amount;
                CurrentHP += amount;
                break;
            case "MP":
                maxMP += amount;
                CurrentMP += amount;
                break;
            case "ATK":
                attack += amount;
                break;
            case "DEF":
                defense += amount;
                break;
            case "SPD":
                speed += amount;
                break;
            default:
                Debug.LogWarning("Stat không tồn tại: " + stat);
                break;
        }
    }
}
