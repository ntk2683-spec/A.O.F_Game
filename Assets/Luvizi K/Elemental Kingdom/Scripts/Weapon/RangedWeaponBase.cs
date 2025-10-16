using UnityEngine;

public abstract class RangedWeaponBase : MonoBehaviour, IWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float shootDelay = 0.5f;
    [SerializeField] protected int baseDamage = 10; // sát thương cơ bản

    protected float nextShootTime = 0f;
    [SerializeField] protected Transform firePoint;

    public abstract void StartAttack();
    public abstract void StopAttack();
}
