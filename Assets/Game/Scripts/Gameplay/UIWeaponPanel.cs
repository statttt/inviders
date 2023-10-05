using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponPanel : MonoBehaviour
{
    public static UIWeaponPanel Instance;

    [SerializeField] private WeaponType _startWeapon;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Weapon> _weaponList = new List<Weapon>();
    [SerializeField] private List<UIWeapon> _uIWeaponList = new List<UIWeapon>();
    public Animator Animator { get { return _animator; }  }

    private WeaponType _weaponType;

    private Weapon _currentWeapon;

    private UIWeapon _currentUIWeapon;

    public WeaponType StartWeapon { get { return _startWeapon; } }

    private void Awake()
    {
        Instance = this;
    }

    public void Activate()
    {
        SetWeapon(Player.Instance.PlayerShooting.CurrentWeapon.WeaponType);
        UpdateWeaponsInfo();
    }

    public void SetWeapon(WeaponType weaponType)
    {
        Weapon weapon = _weaponList.Find(x => x.WeaponType == weaponType);
        if (weapon == null)
        {
            return;
        }
        if (_currentWeapon != null)
        {
            _currentWeapon.Hide();
        }
        _currentWeapon = weapon;
        _currentWeapon.Show();
        UIWeapon uIWeapon = _uIWeaponList.Find(x => x.WeaponType == weaponType);
        if(_currentUIWeapon != null)
        {
            _currentUIWeapon.Deactive();
        }
        _currentUIWeapon = uIWeapon;
        _currentUIWeapon.Active();
        Player.Instance.PlayerShooting.SetWeapon(weaponType);
    }

    public void UpdateWeaponsInfo()
    {
        foreach (UIWeapon weapon in _uIWeaponList)
        {
            if (weapon.gameObject.activeSelf)
            {
                weapon.UpdateInfo(UIUpgrade.Instance.GetLevelWeapon(weapon.WeaponType));
            }
        }
    }
}
