using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Zone : MonoBehaviour
{
    [SerializeField] protected List<Enemy> _enemyList = new List<Enemy>();
    [SerializeField] protected List<Enemy> _enemyAttackList = new List<Enemy>();
    [SerializeField] protected EnemySpawner _enemySpawner;
    [SerializeField] protected List<EnemyFog> _smogList = new List<EnemyFog>();
    [SerializeField] private Nest _nest;
    [SerializeField] private BoxCollider _collider;



    public EnemySpawner EnemySpawner { get { return _enemySpawner; } }


    protected virtual void Start()
    {
        foreach (EnemyFog smog in _smogList)
        {
            smog.Activate();
        }
        if (_nest != null)
        {
            _nest.ResetView();
        }
    }

    public virtual void AddEnemy(Enemy enemy)
    {
        _enemyList.Add(enemy);
    }

    public virtual void RemoveEnemy(Enemy enemy)
    {
        _enemyList.Remove(enemy);
    }

    public virtual void AddAttackEnemy(Enemy enemy)
    {
        _enemyAttackList.Add(enemy);
    }

    public virtual void RemoveAttackEnemy(Enemy enemy)
    {
        _enemyAttackList.Remove(enemy);
    }

    public Vector3 GetRandomPoint(NavMeshAgent agent, NavMeshPath navMeshPath)
    {
        bool isGetCorrectPoint = false;
        Vector3 point = transform.position;
        while (!isGetCorrectPoint)
        {
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(GetRandomPointInCollider(), out navMeshHit, 10f, NavMesh.AllAreas);

            agent.CalculatePath(navMeshHit.position, navMeshPath);
            if(navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                point = navMeshHit.position;
                isGetCorrectPoint = true;
            }
        }
        return point;
    }

    public Vector3 GetRandomPointInCollider()
    {
        var point = new Vector3(
            Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
            Random.Range(_collider.bounds.min.y, _collider.bounds.max.y),
            Random.Range(_collider.bounds.min.z, _collider.bounds.max.z)
        );

        return point;
    }

    public void ShowSmog()
    {
        foreach (EnemyFog smog in _smogList)
        {
            smog.Show();
        }
        if(_nest != null)
        {
            _nest.Show();
        }
    }

    public void HideSmog()
    {
        foreach (EnemyFog smog in _smogList)
        {
            smog.Hide();
        }
        if (_nest != null)
        {
            _nest.Hide();
        }
    }
}
