using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFriendTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private List<Enemy> _enemyFriendsList = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemyFriendsList.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemyFriendsList.Remove(enemy);
        }
    }

    public void ActivateFriends()
    {
        foreach (Enemy enemy in _enemyFriendsList)
        {
            if(enemy != _enemy)
            {
                enemy.Activate(Player.Instance.transform);
            }
        }
        _enemyFriendsList.Clear();
        DeactivateCollider();
    }

    public void ActivateCollider()
    {
        _collider.enabled = true;
    }

    public void DeactivateCollider()
    {
        _collider.enabled = false;
    }

}
