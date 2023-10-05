using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum WeaponType
{
    WT_Riffle,
    WT_Gun,
    WT_Shotgun,
    WT_Uzi
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Transform _spawnBulletPoint;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _sleeveSpawnPoint;
    [SerializeField] private Sleeve _sleevePrefab;
    [SerializeField] private Transform _rightTarget;
    [SerializeField] private Transform _leftTarget;
    [SerializeField] private float _startFireRate;
    [SerializeField] private float _currentFireRate;
    [SerializeField] private float _startDamage;
    [SerializeField] private float _currentDamage;
    [SerializeField] private int _bulletsCount;
    [SerializeField] private float _forceUp;
    [SerializeField] private float _forceRight;

    public int BulletsCount { get { return _bulletsCount; } }

    public Transform SpawnBulletPoint { get { return _spawnBulletPoint; } }
    public WeaponType WeaponType { get { return _weaponType; } }

    public Transform RightTarget { get { return _rightTarget; } }

    public Transform LeftTarget { get { return _leftTarget; } }

    public float FireRate { get { return _currentFireRate; } }
    public float Damage { get { return _currentDamage; } }

    public void Shoot()
    {
        _shootParticle.Play();
        Sleeve sleeve = Instantiate(_sleevePrefab, Level.Instance.transform);
        sleeve.transform.position = _sleeveSpawnPoint.transform.position;
        sleeve.AddForce(transform);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void UpdateWeapon()
    {
        float ratioDamage = (float)Math.Pow(UIUpgrade.Instance.GetRatioWeaponDamage(_weaponType), UIUpgrade.Instance.GetLevelWeapon(_weaponType));
        _currentDamage = _startDamage * ratioDamage;
        float ratiofireRate = (float)Math.Pow(UIUpgrade.Instance.GetRatioWeaponFireRate(_weaponType), UIUpgrade.Instance.GetLevelWeapon(_weaponType));
        _currentFireRate = _startFireRate * ratiofireRate;
    }

    public float GetDamageValue()
    {
        return _currentDamage;
    }

    public float GetFireRateValue()
    {
        return 1f / _currentFireRate;
    }

    public string GetNextDamageValuePercents()
    {
        int percents = (int)(Math.Abs(UIUpgrade.Instance.GetRatioWeaponDamage(_weaponType) - 1f) * 100);
        return percents.ToString();
    }

    public string GetNextFireRateValuePercents()
    {
        int percents = (int)(Math.Abs(UIUpgrade.Instance.GetRatioWeaponFireRate(_weaponType) - 1f)* 100);
        return percents.ToString();
    }

    public Vector3 GetDirection()
    {
        Vector3 direction = transform.forward + transform.up * UnityEngine.Random.Range(-_forceUp,_forceUp) +
            transform.right * UnityEngine.Random.Range(-_forceRight, _forceRight);
        return direction.normalized;
    }

    public void GetDamage()
    {

    }
}
