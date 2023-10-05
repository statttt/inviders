using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEnemyTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private SphereCollider _collider;

    private void Start()
    {
        float radius = _collider.radius;
        _collider.radius = 0f;
        DOTween.To(() => _collider.radius, x  => _collider.radius = x , radius, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            if(enemy.EnemyMovement.isBackToNest)
            {
                return;
            }
            _player.AddEnemy(enemy);
            enemy.IsHavePlayer = true;
            enemy.Activate(_player.transform);
            enemy.EnemyFriendTrigger.ActivateFriends();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            enemy.IsHavePlayer = false;
            _player.RemoveEnemy(enemy);
            if (enemy.IsCanStopped)
            {
                if (enemy.IsInZone && !enemy.IsDestroyer)
                {
                    enemy.Idle();
                }
                else
                {
                    enemy.EnemyMovement.BackToZone();
                }
                enemy.EnemyHealth.HideHealthBar();
            }
            else
            {
                enemy.Activate(_player.transform);
            }
        }
    }

    public void SetRadius(float radius)
    {
        _collider.radius = radius;
    }
}
