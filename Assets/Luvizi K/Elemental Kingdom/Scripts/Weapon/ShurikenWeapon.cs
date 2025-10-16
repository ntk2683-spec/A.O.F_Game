using UnityEngine;
using System.Collections;

public class ShurikenWeapon : RangedWeaponBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject buffBulletPrefab;
    [SerializeField] private Joystick joystick;
    private Transform currentTarget;
    private Coroutine shootingCoroutine;
    private float resetDelay = 2f;
    private int shootCount = 0;
    private float lastShootTime = 0f;
    public PlayerBase shooter; // lấy stats từ PlayerBase
    void Start()
    {
        if (shooter == null)
            shooter = GetComponentInParent<PlayerBase>();
    }

    void Update()
    {
        currentTarget = FindClosestEnemy();

        if (currentTarget != null)
        {
            Vector2 dir = currentTarget.position - firePoint.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void StartAttack()
    {
        if (shootingCoroutine == null)
            shootingCoroutine = StartCoroutine(ShootContinuously());
    }

    public override void StopAttack()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            TryShoot();
            yield return null;
        }
    }

    private void TryShoot()
    {
        if (Time.time < nextShootTime)
            return;

        if (Time.time - lastShootTime > resetDelay)
            shootCount = 0;

        Shoot();
        nextShootTime = Time.time + shootDelay;
    }

    private void Shoot()
    {
        Vector2 dir;


        if (currentTarget != null)
        {
            dir = (currentTarget.position - firePoint.position).normalized;
            PlayerControl.Instance.FaceTarget(currentTarget.position); // gọi trực tiếp
        }
        else if (joystick != null && joystick.Direction.sqrMagnitude > 0.1f)
        {
            dir = joystick.Direction.normalized;
            PlayerControl.Instance.FaceDirection(dir); // gọi trực tiếp
        }
        else
        {
            SpriteRenderer sr = shooter.GetComponent<SpriteRenderer>();
            float facing = sr.flipX ? -1f : 1f;
            dir = new Vector2(facing, 0f).normalized;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        shootCount++;
        GameObject prefabToShoot = (shootCount >= 4) ? buffBulletPrefab : bulletPrefab;
        if (shootCount >= 4) shootCount = 0;

        GameObject bullet = Instantiate(prefabToShoot, firePoint.position, rot);

        BulletBase bulletScript = bullet.GetComponent<BulletBase>();
        if (bulletScript != null)
        {
            bulletScript.Init(dir, shooter.ATK, shooter.gameObject);
        }

        // 🔥 Gọi animation Attack
        if (PlayerControl.Instance != null)
        {
            PlayerControl.Instance.PlayAttackAnimation();
        }

        lastShootTime = Time.time;
    }


    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist <= attackRange)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
