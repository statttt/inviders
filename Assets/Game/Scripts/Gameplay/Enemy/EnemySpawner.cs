using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _enemySpawnPoint;

    public Enemy CreateEnemy(EnemyZone zone,int level)
    {
        Enemy enemy = Instantiate(_enemyPrefab, Level.Instance.transform);
        //float randomX = Random.Range(-5, 5);
        //float randomZ = Random.Range(-5, 5);
        enemy.transform.position = transform.position;
        //enemy.transform.rotation = _enemySpawnPoint.rotation;
        enemy.SetParameters(zone, level);
        enemy.EnemyMovement.BackToZone();
        return enemy;
    }
}
