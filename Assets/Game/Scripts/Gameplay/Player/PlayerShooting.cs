using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private WeaponType _startWeapon;
    [SerializeField] private PlayerEnemyTrigger _enemyTrigger;
    [SerializeField] private float _startRadius;
    [SerializeField] private float _currentRadius;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();
    [SerializeField] private int _countForCreateBullets;
    [SerializeField] private BulletInfo _bulletInfo;
    [SerializeField] private List<Bullet> _bullets = new List<Bullet>();
    [SerializeField] private float _damage;
    [SerializeField] private float _delayBetweenShoot;
    [SerializeField] private float _shootCameraShakeDuration;
    [SerializeField] private float _shootCameraShakeMagnitude;


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
        foreach (Weapon weapon in _weaponList)
        {
            weapon.UpdateWeapon();
        }
        if (!PlayerPrefs.HasKey("WeaponInHand"))
        {
            WeaponInHand = (int)_startWeapon;
        }
        SetWeapon((WeaponType)WeaponInHand);
        CreateBullets(_countForCreateBullets);
    }

    public void SetWeapon(WeaponType weaponType)
    {
        Weapon weapon = _weaponList.Find(x => x.WeaponType == weaponType);
        if(weapon == null)
        {
            return;
        }
        if(_currentWeapon != null)
        {
            _currentWeapon.Hide();
        }
        _currentWeapon = weapon;
        _currentWeapon.Show();
        _currentWeapon.UpdateWeapon();
        Player.Instance.PlayerAnimator.SetHandsPoints(weapon.RightTarget, weapon.LeftTarget);
        _delayBetweenShoot = weapon.FireRate;
        _damage = weapon.Damage;
        WeaponInHand = (int)_currentWeapon.WeaponType;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    SetWeapon(WeaponType.WT_Riffle);
        //}
        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    SetWeapon(WeaponType.WT_Gun);
        //}
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    SetWeapon(WeaponType.WT_Shotgun);
        //}
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    SetWeapon(WeaponType.WT_Uzi);
        //}
        if(_timer < _delayBetweenShoot)
        {
            _timer += Time.deltaTime;
        }
    }



    public void FixedUpdate()
    {
        if (Target && !Player.Instance.PlayerMovement.IsMoving)
        {
            Player.Instance.PlayerMovement.RotateToTarget(Target.transform.position);
        }

    }

    public void LateUpdate()
    {
        if (Player.Instance.IsDie)
        {
            Player.Instance.PlayerAnimator.isActiveIK = false;
            return;
        }
        if (Target != null)
        {
            Player.Instance.PlayerAnimator.isActiveIK = true;
            if (_timer >= _delayBetweenShoot)
            {
                float angle = Vector3.Angle(Target.transform.position - transform.position, transform.forward);
                if (angle <= 10 && angle >= -10)
                {
                    Shoot();
                    _timer = 0;
                }
            }
        }
        if (!Player.Instance.PlayerMovement.IsMoving)
        {
            Player.Instance.PlayerAnimator.IdleIK();
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
        CameraMovement.Instance.Shake(_shootCameraShakeDuration, _shootCameraShakeMagnitude);
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

    internal void UpdateRadius()
    {
        float ratio = (float)Math.Pow(UIUpgrade.Instance.ratioRadiusPlayer, UIUpgrade.Instance.LevelPlayerRadius);
        _currentRadius = _startRadius * ratio;
        _enemyTrigger.SetRadius(_currentRadius);
    }

    public void UpdateWeponByType(WeaponType weaponType)
    {
        Weapon weapon = _weaponList.Find(x => x.WeaponType == weaponType);
        if (weapon != null)
        {
            weapon.UpdateWeapon();
        }
    }

    public Weapon GetWeponByType(WeaponType weaponType)
    {
        Weapon weapon = _weaponList.Find(x => x.WeaponType == weaponType);
        return weapon;
    }
}
