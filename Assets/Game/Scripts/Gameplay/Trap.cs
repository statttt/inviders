using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private Animator _animator;

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            if (!enemy.IsDie)
            {
                _animator.Rebind();
                _animator.Play("Attack");
                enemy.EnemyHealth.GetDamage(_damage, -enemy.transform.forward);
            }
        }
    }
}
