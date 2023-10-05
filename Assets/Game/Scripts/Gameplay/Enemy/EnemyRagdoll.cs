using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private List<Rigidbody> _rbList = new List<Rigidbody>();
    [SerializeField] private List<Collider> _colliderList = new List<Collider>();
    [SerializeField] private float _timeForDeactivate;
    [SerializeField] private float _timeForDestroy;
    [SerializeField] private float _dieForce;

    private void Start()
    {
        DeactivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        foreach (Rigidbody rb in _rbList)
        {
            rb.isKinematic = false;
            rb.velocity = _enemy.transform.forward * -1 * _dieForce;
        }
        StartCoroutine(IDeactivate());
    }

    public void DeactivateRagdoll()
    {
        foreach (Rigidbody rb in _rbList)
        {
            rb.isKinematic = true;
        }

    }

    private IEnumerator IDeactivate()
    {
        yield return new WaitForSeconds(_timeForDeactivate);
        foreach (Collider collider in _colliderList)
        {
            collider.isTrigger = true;
        }
        yield return new WaitForSeconds(_timeForDestroy);
        Destroy(_enemy.gameObject);

    }
}
