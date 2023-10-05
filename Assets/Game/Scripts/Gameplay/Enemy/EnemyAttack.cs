using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _damage;

    public Player Player { get; set; }
    public EnvironmentZone Environment { get; set; }
    public bool IsHavePlayer;
    public bool IsHaveEnvironment;

    public EnemyAttackTrigger EnemyAttackTrigger { get; set; }

    public float GetDamage() => _damage;
    
    public void SetDamage(int level)
    {
        _damage *= level;
    }

    public void TryPlayer()
    {
        if (Player && Player.IsDie)
        {
            _enemy.Idle();
            return;
        }

        if (!IsHavePlayer)
        {
            if (_enemy.IsHavePlayer)
            {
                _enemy.Activate(Player.Instance.transform);
            }
            else
            {
                _enemy.EnemyMovement.BackToZone();
            }
        }
    }

    public void LookAtTarget()
    {
        if (_enemy.IsHavePlayer)
        {
            Vector3 targetPosition = new Vector3(Player.Instance.transform.position.x, transform.position.y, Player.Instance.transform.position.z);
            transform.LookAt(targetPosition);
        }
        else if (IsHaveEnvironment)
        {
            Vector3 targetPosition = new Vector3(Environment.transform.position.x, transform.position.y, Environment.transform.position.z);
            transform.LookAt(targetPosition);
        }
    }

    public void Attack()
    {
        if(IsHavePlayer)
        {
            Player.PlayerHealth.GetDamage(_enemy.EnemyAttack.GetDamage());
        }else if (IsHaveEnvironment)
        {
            Environment.Zone.GetDamage(_enemy.EnemyAttack.GetDamage());
        }
    }

    public bool TryCanAttack()
    {
        if (IsHaveEnvironment)
        {
            _enemy.Attack();
            return true;
        }
        else
        {
            return false;
        }
    }

}
