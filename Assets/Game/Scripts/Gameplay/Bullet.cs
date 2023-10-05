using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _bloodPrefab;
    private Transform _parent;
    private Rigidbody _rigidbody;
    private float _speed;
    private float _damage;

    public void UpdateBullet(BulletInfo bulletInfo, Transform parent, float damage)
    {
        _speed = bulletInfo.Speed;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
        _parent = parent;
    }

    public void Activate(Vector3 direction,float damage)
    {
        _damage = damage;
        _trail.Clear();
        transform.position = _parent.position;
        transform.rotation = _parent.rotation;
        transform.rotation = Quaternion.LookRotation(direction);
        gameObject.SetActive(true);
        StartCoroutine(IDestroy());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            _rigidbody.velocity = transform.forward * _speed;
        }
    }

    private IEnumerator IDestroy()
    {
        yield return new WaitForSeconds(3);
        if (gameObject.activeSelf)
        {
            DestroyMySelf();
        }
    }

    public void DestroyMySelf()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            ParticleSystem particle = Instantiate(_bloodPrefab, transform);
            particle.transform.parent = Level.Instance.transform;
            Destroy(particle.gameObject, 2);
            enemy.EnemyMovement.Hit(transform.forward);
            enemy.EnemyHealth.GetDamage(_damage, transform.forward);
        }
        DestroyMySelf();
    }
}
