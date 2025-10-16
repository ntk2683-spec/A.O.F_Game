using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour startingWeapon; // gắn RangedWeapon, MeleeWeapon, ...
    private IWeapon currentWeapon;

    void Awake()
    {
        currentWeapon = startingWeapon as IWeapon;
    }

    public void StartAttack()
    {
        currentWeapon?.StartAttack();
    }

    public void StopAttack()
    {
        currentWeapon?.StopAttack();
    }

    public void EquipWeapon(IWeapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
}

