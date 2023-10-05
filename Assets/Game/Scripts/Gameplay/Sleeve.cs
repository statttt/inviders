using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleeve : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force;
    [SerializeField] private float _forceUp;
    [SerializeField] private float _forceRight;

    public void AddForce(Transform dirTransform)
    {

        Vector3 direction = dirTransform.right * _forceRight + dirTransform.up * _forceUp;
        _rb.AddForce(direction * _force, ForceMode.Impulse);
        _rb.AddTorque(direction* _forceRight, ForceMode.Impulse);
        Destroy(gameObject, 2);
    }
}
