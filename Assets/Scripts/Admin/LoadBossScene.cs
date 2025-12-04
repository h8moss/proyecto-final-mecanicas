using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemySpawner))]
public class LoadBossScene : MonoBehaviour
{
    EnemySpawner spawner;
    void Start()
    {
        spawner = GetComponent<EnemySpawner>();
        spawner.onFinalWaveEnd += LoadBoss;
    }

    void LoadBoss()
    {
        SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
