using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyShooting : MonoBehaviour
{
    [SerializeField] private Ally _ally;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private int _countForCreateBullets;
    [SerializeField] private BulletInfo _bulletInfo;
    [SerializeField] private List<Bullet> _bullets = new List<Bullet>();
    [SerializeField] private float _damage;
    [SerializeField] private float _delayBetweenShoot;
    [SerializeField] private float _speedRotate;


    private float _timer;

    public Enemy Target { get; set; }

    public Weapon CurrentWeapon { get { return _currentWeapon; } }

    public int WeaponInHand
    {
        get => PlayerPrefs.GetInt("WeaponInHand", 0);
        set => PlayerPrefs.SetInt("WeaponInHand", value);
    }

    private void Start()
    {
        CreateBullets(_countForCreateBullets);
    }


    private void Update()
    {
        if (_timer < _delayBetweenShoot)
        {
            _timer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (Target != null && !_ally.IsFear)
        {
            Quaternion direction = Quaternion.LookRotation(Target.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, _speedRotate);
        }
    }


    public void LateUpdate()
    {
        _currentWeapon.gameObject.SetActive(true);
        _ally.FindEnemy();
        if (Target != null)
        {
            //transform.LookAt(Target.transform.position);
            if (_ally.IsFear)
            {
                _currentWeapon.gameObject.SetActive(false);
                return;
            }
            else
            {
                if (_timer >= _delayBetweenShoot)
                {
                    if (Vector3.Angle(Target.transform.position - transform.position, transform.forward) <= 10)
                    {
                        Shoot();
                        _timer = 0;
                    }
                }
            }
        }
    }


    public void CreateBullets(int bulletsCount = 10)
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            Bullet bullet = Instantiate(_bulletInfo.BulletPrefab, Level.Instance.transform);
            bullet.UpdateBullet(_bulletInfo, _currentWeapon.SpawnBulletPoint, _damage);
            _bullets.Add(bullet);

        }
    }

    public void Shoot()
    {
        _currentWeapon.Shoot();
        for (int i = 0; i < _currentWeapon.BulletsCount; i++)
        {
            ShowBullet();
        }
    }

    public void ShowBullet()
    {
        bool isHaveBulet = false;
        foreach (Bullet bullet in _bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.Activate(_currentWeapon.GetDirection(), _damage / _currentWeapon.BulletsCount);
                isHaveBulet = true;
                break;
            }
        }
        if (!isHaveBulet)
        {
            CreateBullets();
            ShowBullet();
        }
    }

    public void SetParameters(float damage, float delayBetweenShoot)
    {
        _damage = damage;
        _delayBetweenShoot = delayBetweenShoot;
    }

}
