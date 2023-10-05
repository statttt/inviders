using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemysList = new List<Enemy>();
    [SerializeField] private AllyShooting _allyShooting;
    [SerializeField] private AllyAnimator _allyAnimator;

    public AllyShooting AllyShooting { get { return _allyShooting;} }
    public AllyAnimator AllyAnimator { get { return _allyAnimator; } }

    public bool IsFear;

    public void AddEnemy(Enemy enemy)
    {
        _enemysList.Add(enemy);
        enemy.AddAlly(this);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemysList.Remove(enemy);
    }

    public void FindEnemy()
    {
        _allyShooting.Target = null;
        if (_enemysList.Count > 0)
        {
            float distance = float.MaxValue;
            foreach (Enemy enemy in _enemysList)
            {
                if(enemy != null)
                {
                    float newDistance = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        _allyShooting.Target = enemy;
                    }
                }
            }
        }
    }
}
