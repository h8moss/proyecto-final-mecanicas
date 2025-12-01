using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private static readonly WaitForSeconds waitFor1Second = new(1);
    [SerializeField] private GameObject fireEnemy;
    [SerializeField] private GameObject iceEnemy;
    [SerializeField] private GameObject waterEnemy;
    [SerializeField] private GameObject earthEnemy;
    [SerializeField] private GameObject lightningEnemy;

    [SerializeField] private EnemyWave[] waves;
    [SerializeField] private bool isInfinite;

    private GameObject[] fireEnemies;
    private GameObject[] iceEnemies;
    private GameObject[] waterEnemies;
    private GameObject[] earthEnemies;
    private GameObject[] lightningEnemies;

    private int currentEnemies;

    public delegate void OnWaveEnd(int wave);
    public delegate void OnFinalWaveEnd();
    
    public OnWaveEnd onWaveEnd;
    public OnFinalWaveEnd onFinalWaveEnd;

    void Start()
    {
        GetEnemyCounts();
        InstantiateEnemies();
        StartCoroutine(WaveCoroutine());
    }

    private void GetEnemyCounts()
    {
        int fireEnemyCount = 0;
        int waterEnemyCount = 0;
        int iceEnemyCount = 0;
        int earthEnemyCount = 0;
        int lightningEnemyCount = 0;

        foreach (var wave in waves)
        {
            fireEnemyCount = Mathf.Max(fireEnemyCount, wave.fireEnemyCount);
            waterEnemyCount = Mathf.Max(waterEnemyCount, wave.waterEnemyCount);
            iceEnemyCount = Mathf.Max(iceEnemyCount, wave.iceEnemyCount);
            earthEnemyCount = Mathf.Max(earthEnemyCount, wave.earthEnemyCount);
            lightningEnemyCount = Mathf.Max(lightningEnemyCount, wave.lightningEnemyCount);
        }
        fireEnemies = new GameObject[fireEnemyCount];
        waterEnemies = new GameObject[waterEnemyCount];
        iceEnemies = new GameObject[iceEnemyCount];
        earthEnemies = new GameObject[earthEnemyCount];
        lightningEnemies = new GameObject[lightningEnemyCount];
    }

    private void InstantiateEnemies()
    {
        for (int i=0; i<fireEnemies.Length; i++)
        {
            fireEnemies[i] = Instantiate(fireEnemy);
            fireEnemies[i].SetActive(false);
            fireEnemies[i].GetComponent<EnemyHealthControler>().onDeath += OnEnemyDied;
        }
        for (int i=0; i<iceEnemies.Length; i++)
        {
            iceEnemies[i] = Instantiate(iceEnemy);
            iceEnemies[i].SetActive(false);
            iceEnemies[i].GetComponent<EnemyHealthControler>().onDeath += OnEnemyDied;
        }
        for (int i=0; i<earthEnemies.Length; i++)
        {
            earthEnemies[i] = Instantiate(earthEnemy);
            earthEnemies[i].SetActive(false);
            earthEnemies[i].GetComponent<EnemyHealthControler>().onDeath += OnEnemyDied;
        }
        for (int i=0; i<waterEnemies.Length; i++)
        {
            waterEnemies[i] = Instantiate(waterEnemy);
            waterEnemies[i].SetActive(false);
            waterEnemies[i].GetComponent<EnemyHealthControler>().onDeath += OnEnemyDied;
        }
        for (int i=0; i<lightningEnemies.Length; i++)
        {
            lightningEnemies[i] = Instantiate(lightningEnemy);
            lightningEnemies[i].SetActive(false);
            lightningEnemies[i].GetComponent<EnemyHealthControler>().onDeath += OnEnemyDied;
        }
    }

    IEnumerator WaveCoroutine()
    {
        for (int currentWave=0; isInfinite || currentWave < waves.Length; currentWave++)
        {
            int waveIndex = currentWave % waves.Length; 
            EnemyWave wave = waves[waveIndex];

            // Wait start time
            yield return new WaitForSeconds(wave.breakTime);

            // Spawn enemies
            int fireEnemyCount = 0;
            int waterEnemyCount = 0;
            int iceEnemyCount = 0;
            int earthEnemyCount = 0;
            int lightningEnemyCount = 0;

            while (currentEnemies < wave.TotalEnemies)
            {
                if (fireEnemyCount < wave.fireEnemyCount && Random.value > 0.5)
                {
                    SpawnEnemy(fireEnemies);
                    yield return new WaitForSeconds(Random.Range(wave.randomTimeBetweenSpawns.x, wave.randomTimeBetweenSpawns.y));
                    fireEnemyCount++;
                    currentEnemies++;
                }
                if (iceEnemyCount < wave.iceEnemyCount && Random.value > 0.5)
                {
                    SpawnEnemy(iceEnemies);
                    yield return new WaitForSeconds(Random.Range(wave.randomTimeBetweenSpawns.x, wave.randomTimeBetweenSpawns.y));
                    iceEnemyCount++;
                    currentEnemies++;
                }
                if (waterEnemyCount < wave.waterEnemyCount && Random.value > 0.5)
                {
                    SpawnEnemy(waterEnemies);
                    yield return new WaitForSeconds(Random.Range(wave.randomTimeBetweenSpawns.x, wave.randomTimeBetweenSpawns.y));
                    waterEnemyCount++;
                    currentEnemies++;
                }
                if (earthEnemyCount < wave.earthEnemyCount && Random.value > 0.5)
                {
                    SpawnEnemy(earthEnemies);
                    yield return new WaitForSeconds(Random.Range(wave.randomTimeBetweenSpawns.x, wave.randomTimeBetweenSpawns.y));
                    earthEnemyCount++;
                    currentEnemies++;
                }
                if (lightningEnemyCount < wave.lightningEnemyCount && Random.value > 0.5)
                {
                    SpawnEnemy(lightningEnemies);
                    yield return new WaitForSeconds(Random.Range(wave.randomTimeBetweenSpawns.x, wave.randomTimeBetweenSpawns.y));
                    lightningEnemyCount++;
                    currentEnemies++;
                }
            }

            while (currentEnemies > 0) // while wave hasnt ended
            {
                yield return waitFor1Second;
            }

            onWaveEnd?.Invoke(waveIndex);
            if (waveIndex == waves.Length-1 && !isInfinite)
            {
                onFinalWaveEnd?.Invoke();
            }
        }
    }

    void SpawnEnemy(GameObject[] enemies)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Vector3 spawnLocation = children[Random.Range(0, children.Length)].position;
        if (NavMesh.SamplePosition(spawnLocation, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            spawnLocation = hit.position;
        }
        foreach (var enemy in enemies) {
            if (!enemy.activeInHierarchy) 
            {
                enemy.SetActive(true);
                if (enemy.TryGetComponent(out PooledObjectHelper poh)) {
                    poh.Reset();
                }
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.Warp(spawnLocation);
                break;
            }
        }
    }

    void OnEnemyDied()
    {
        currentEnemies--;
    }
}
