using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private float _startColliderRadius;
    [SerializeField] private float _maxColliderRadius;

    private void Start()
    {
        _enemy.EnemyAttack.EnemyAttackTrigger = this;
        StartColliderRadius();
    }

    public void StartColliderRadius()
    {
        _collider.radius = _startColliderRadius;
    }

    public void MaxColliderRadius()
    {
        _collider.radius = _maxColliderRadius;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player) && !player.IsDie)
        {
            _enemy.EnemyAttack.Player = player;
            _enemy.EnemyAttack.IsHavePlayer = true;
            MaxColliderRadius();
            _enemy.Attack();
            return;
        }
        if (other.TryGetComponent(out EnvironmentZone environment) && _enemy.IsDestroyer)
        {
            if(environment.Zone == _enemy.Zone)
            {
                if(_enemy.EnemyMovement.Target != Player.Instance.transform)
                {
                    _enemy.EnemyMovement.Target = null;
                }
                _enemy.EnemyAttack.Environment = environment;
                _enemy.EnemyAttack.IsHaveEnvironment = true;
                if (!_enemy.IsHavePlayer)
                {
                    _enemy.Attack();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _enemy.EnemyAttack.IsHavePlayer = false;
            StartColliderRadius();
        }
        if (other.TryGetComponent(out EnvironmentZone environment) && _enemy.IsDestroyer)
        {
            if (environment.Zone == _enemy.Zone)
            {
                _enemy.EnemyAttack.IsHaveEnvironment = false;
                if (!_enemy.IsHavePlayer)
                {
                    _enemy.EnemyMovement.BackToZone();
                }
            }
        }
    }
}
