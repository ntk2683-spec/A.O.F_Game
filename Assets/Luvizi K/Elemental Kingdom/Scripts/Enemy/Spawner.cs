using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int totalToSpawn = 1000;
    [SerializeField] private int maxAlive = 100;
    [SerializeField] private float range = 1000f;

    [Header("Spawn overlap check (2D)")]
    [SerializeField] private float spawnCheckRadius = 0.5f;
    [SerializeField] private LayerMask spawnCollisionMask = Physics2D.DefaultRaycastLayers;
    [SerializeField] private int maxSpawnPositionAttempts = 30;

    private int spawnedCount = 0;
    [SerializeField] private int killedCount = 0;
    private int RemainingEnemies => Mathf.Max(0, totalToSpawn - killedCount);
    [Header("UI")]
    [SerializeField] private TMP_Text killedText;

    private bool isSpawningComplete = false;

    // Track alive enemy instances by InstanceID to prevent double-counting
    private HashSet<int> aliveEnemies = new HashSet<int>();

    private const string RemainingKey = "RemainingEnemies";
    private const string SpawnedKey = "SpawnedEnemies";
    private const string KilledKey = "KilledEnemies";
    // Gọi khi enemy chết để lưu tiến trình
    private void SaveProgress()
    {
        PlayerPrefs.SetInt(KilledKey, killedCount);
        PlayerPrefs.Save();
    }


    // Load dữ liệu khi vào game
    private void LoadProgress()
    {
        killedCount = PlayerPrefs.GetInt(KilledKey, 0);
    }



    void Start()
    {
        LoadProgress();   // 🔥 phải load dữ liệu đã lưu trước
        UpdateKilledText();

        int firstSpawn = Mathf.Min(maxAlive, RemainingEnemies);
        for (int i = 0; i < firstSpawn; i++)
            SpawnEnemy();

        if (spawnedCount >= totalToSpawn)
            isSpawningComplete = true;
    }

    private void SpawnEnemy()
    {
        if (spawnedCount >= totalToSpawn)
        {
            isSpawningComplete = true;
            return;
        }

        Vector2 spawnPos;
        if (!TryGetSpawnPosition(out spawnPos))
        {
            // fallback: nếu không tìm được vị trí "rỗi" sau nhiều lần thử, spawn bất kỳ vị trí nào
            spawnPos = new Vector2(
                UnityEngine.Random.Range(-range / 2f, range / 2f),
                UnityEngine.Random.Range(-range / 2f, range / 2f)
            );
            Debug.LogWarning($"Spawner: couldn't find empty spawn spot after {maxSpawnPositionAttempts} tries. Spawning anyway.");
        }

        GameObject enemy = Instantiate(enemyPrefab, (Vector3)spawnPos, Quaternion.identity);
        int id = enemy.GetInstanceID();

        // mark as alive
        aliveEnemies.Add(id);
        spawnedCount++;

        // try to subscribe to enemy death event robustly
        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript != null)
        {
            // create a handler that calls internal processor and unsubscribes itself
            Action handler = null;
            handler = () =>
            {
                OnEnemyDiedInternal(id);
                enemyScript.OnEnemyDeath -= handler;
            };
            enemyScript.OnEnemyDeath += handler;
        }
        else
        {
            // fallback: attach notifier which will call back spawner on Destroy / if no event present
            var notifier = enemy.AddComponent<SpawnerEnemyNotifier>();
            notifier.Init(this, id);
        }

        if (spawnedCount >= totalToSpawn)
            isSpawningComplete = true;
    }

    private bool TryGetSpawnPosition(out Vector2 result)
    {
        for (int i = 0; i < maxSpawnPositionAttempts; i++)
        {
            float x = UnityEngine.Random.Range(-range / 2f, range / 2f);
            float y = UnityEngine.Random.Range(-range / 2f, range / 2f);
            Vector2 pos = new Vector2(x, y);

            // 2D overlap check - nếu project của bạn là 3D thì đổi sang Physics.OverlapSphere
            Collider2D hit = Physics2D.OverlapCircle(pos, spawnCheckRadius, spawnCollisionMask);
            if (hit == null)
            {
                result = pos;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    // Core logic when an enemy dies (ensures single-processing per instance)
    private void OnEnemyDiedInternal(int id)
    {
        if (!aliveEnemies.Remove(id)) return;

        killedCount = Mathf.Min(totalToSpawn, killedCount + 1); // clamp tại đây
        UpdateKilledText();
        SaveProgress();

        if (!isSpawningComplete && spawnedCount < totalToSpawn && aliveEnemies.Count < maxAlive)
        {
            SpawnEnemy();
        }

        if (RemainingEnemies <= 0 && aliveEnemies.Count == 0)
        {
            Debug.Log("All enemies defeated! You win!");
            PlayerPrefs.DeleteKey(KilledKey);
            PlayerPrefs.Save();
            WinGameScript.Instance?.WinGame();
        }
    }
    private void UpdateKilledText()
    {
        if (killedText != null)
        {
            killedText.text = $"Alive: {RemainingEnemies}";
        }
    }


    // public fallback để notifier gọi
    public void NotifyEnemyDeath(int id) => OnEnemyDiedInternal(id);
}

/// <summary>
/// Fallback component: nếu prefab không expose event hoặc event không dùng được,
/// component này sẽ gọi spawner.NotifyEnemyDeath khi object bị Destroy hoặc enemy's event firing.
/// </summary>
public class SpawnerEnemyNotifier : MonoBehaviour
{
    private Spawner spawner;
    private int id;
    private EnemyBase enemyBase;

    public void Init(Spawner spawner, int id)
    {
        this.spawner = spawner;
        this.id = id;
        enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
            enemyBase.OnEnemyDeath += OnEnemyDeathInternal;
    }

    private void OnEnemyDeathInternal()
    {
        NotifyAndCleanup();
    }

    private void OnDestroy()
    {
        // nếu object bị destroy mà event chưa fire, vẫn notify 1 lần
        NotifyAndCleanup();
    }

    private void NotifyAndCleanup()
    {
        if (spawner != null)
        {
            spawner.NotifyEnemyDeath(id);
            spawner = null;
        }

        if (enemyBase != null)
            enemyBase.OnEnemyDeath -= OnEnemyDeathInternal;
    }
}
