using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // empty object trên Player để gắn vũ khí
    private RangedWeaponBase currentWeapon;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
        currentWeapon = newWeapon.GetComponent<RangedWeaponBase>();
    }

    public void StartAttack()
    {
        currentWeapon?.StartAttack();
    }

    public void StopAttack()
    {
        currentWeapon?.StopAttack();
    }
}
