using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;


    [SerializeField] private float _speed;
    [SerializeField] private float _hitForce;

    private NavMeshAgent _agent;
    [SerializeField] private bool _isBackToNest;

    private NavMeshPath _navMeshPath;

    public bool IsMove { get; set; }
    public Transform Target 
    { 
        get; 

        set; 
    }

    private Transform _lastTarget;

    public bool isGoToHome { get; set; }
    public bool isBackToNest { get { return _isBackToNest; } }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _navMeshPath = new NavMeshPath();
    }

    public void Move()
    {
        if (Player.Instance && Player.Instance.IsDie)
        {
            _enemy.Idle();
        }
        if (_agent.enabled)
        {
            if (isBackToNest)
            {
                if (_agent.enabled && (_agent.destination - transform.position).sqrMagnitude <= _agent.stoppingDistance * _agent.stoppingDistance)
                {
                    _enemy.EnemyHealth.Deactivate();
                    Player.Instance.RemoveEnemy(_enemy);
                    Destroy(_enemy.gameObject);
                }

            }else if (Target != null)
            {
                _agent.SetDestination(Target.position);

            }
            else if (isGoToHome)
            {
                if (_agent.enabled && (_agent.destination - transform.position).sqrMagnitude <= _agent.stoppingDistance * _agent.stoppingDistance)
                {
                    isGoToHome = false;
                    _enemy.Idle();
                    _enemy.EnemyFriendTrigger.ActivateCollider();
                }
            }
        }
    }

    public void Stop()
    {
        if (_agent != null && _agent.enabled)
        {
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
        }
        Target = null;
    }


    public void ActivateAgnet()
    {
        _agent.enabled = true;
    }

    public void DeactivateAgent()
    {
        if(_agent != null)
        {
            _agent.enabled = false;
        }
    }

    public void Hit(Vector3 direction)
    {
        _agent.velocity = direction * _hitForce;
    }

    internal void LookAtTarget()
    {
        //transform.rotation = Quaternion.LookRotation(_agent.velocity, Vector3.up);
    }

    internal void BackToZone()
    {
        if (!_agent.enabled)
        {
            return;
        }
        if (isBackToNest)
        {
            _agent.SetDestination(_enemy.Zone.EnemySpawner.transform.position);
            return;
        }
        if (_enemy.IsDestroyer)
        {
            if (!_enemy.EnemyAttack.TryCanAttack())
            {
                _enemy.Run();
                Target = _enemy.TargetForDestroy;
                _agent.SetDestination(Target.position);
            }
        }
        else
        {
            _enemy.Run();
            Target = null;
            isGoToHome = true;
            _agent.SetDestination(_enemy.Zone.GetRandomPoint(_agent, _navMeshPath));
        }
    }

    internal void BackToNest()
    {
        _enemy.Collider.enabled = false;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        Target = null;
        _enemy.Run();
        _isBackToNest = true;
        BackToZone();
    }

    public void ResetPath()
    {
        _agent.ResetPath();
    }

}
