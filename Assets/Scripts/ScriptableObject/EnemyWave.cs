using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public float breakTime;
    public Vector2 randomTimeBetweenSpawns;
    public int fireEnemyCount;
    public int iceEnemyCount;
    public int waterEnemyCount;
    public int earthEnemyCount;
    public int lightningEnemyCount;

    public int TotalEnemies
    {
        get => fireEnemyCount + iceEnemyCount + waterEnemyCount + earthEnemyCount + lightningEnemyCount;
    }
}
