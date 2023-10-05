using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerForAttack : MonoBehaviour
{
    [SerializeField] private EnemyZone _enemyZone;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private List<Transform> _enemySpawnPointList = new List<Transform>();
    [SerializeField] private int _maxCountAttackEnemy;
    [SerializeField] private float _delayBetweenCreateEnemy;

    private Enemy CreateEnemyForAttack(int level)
    {
        Enemy enemy = Instantiate(_enemyPrefab, Level.Instance.transform);
        enemy.transform.position = _enemySpawnPointList[Random.Range(0, _enemySpawnPointList.Count)].position;
        enemy.SetParametersForDestroyer(_enemyZone, level, _enemyZone.EnvironmentList[Random.Range(0, _enemyZone.EnvironmentList.Count)].transform);
        _enemyZone.AddAttackEnemy(enemy);
        return enemy;
    }

    public Enemy CreateEnemyForAttackHelp(int level, EnemyZone zone)
    {
        Enemy enemy = Instantiate(_enemyPrefab, Level.Instance.transform);
        enemy.transform.position = _enemyZone.EnemySpawner.transform.position;
        enemy.SetParametersForDestroyer(zone, level, zone.EnvironmentList[Random.Range(0, zone.EnvironmentList.Count)].transform);
        zone.AddAttackEnemy(enemy);
        return enemy;
    }

    public void CreateForAttack()
    {
        StartCoroutine(ICreateForAttack());
    }

    private IEnumerator ICreateForAttack()
    {
        for (int i = 0; i < _maxCountAttackEnemy; i++)
        {
            CreateEnemyForAttack(Random.Range(_enemyZone.NumberZone, _enemyZone.NumberZone + 2));
            yield return new WaitForSeconds(_delayBetweenCreateEnemy);
        }
    }
}
