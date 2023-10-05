using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyEnemyTrigger : MonoBehaviour
{
    [SerializeField] private Ally _ally;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.EnemyMovement.isBackToNest)
            {
                return;
            }
            _ally.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _ally.RemoveEnemy(enemy);
            enemy.RemoveAlly(_ally);
        }
    }

}
