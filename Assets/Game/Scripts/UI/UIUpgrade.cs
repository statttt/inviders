using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public enum UpgradeType
{
    UT_PlayerHealth,
    UT_PlayerSpeed,
    UT_PlayerRadius,
    UT_ZoneHealth,
    UT_ZoneTrap,
    UT_ZoneAllies
}

public class UIUpgrade : UIElement
{
    public static UIUpgrade Instance;

    [SerializeField] private List<UIButtonUpgrade> _buttonsList = new List<UIButtonUpgrade>();
    [SerializeField] private Animator _playerPanel;
    [SerializeField] private Animator _zonePanel;
    [SerializeField] private UIWeaponPanel _weaponPanel;
    [SerializeField] private Animator _animator;
    public float ratioHealthPlayer;
    public float ratioSpeedPlayer;
    public float ratioRadiusPlayer;
    public float ratioHealthZone;
    public float ratioTrapZone;
    public float ratioAlliesDamageZone;
    public float ratioAlliesFireRateZone;
    public float ratioWeaponDamageGun;
    public float ratioWeaponFireRateGun;
    public float ratioWeaponDamageUzi;
    public float ratioWeaponFireRateUzi;
    public float ratioWeaponDamageShotGun;
    public float ratioWeaponFireRateShotGun;
    public float ratioWeaponDamageRiffle;
    public float ratioWeaponFireRateRiffle;
    private bool _isActive;
    private EnemyZone _zone;

    public EnemyZone Zone { get { return _zone; } }

    public int LevelPlayerHealth
    {
        get => PlayerPrefs.GetInt("LevelPlayerHealth", 0);
        set => PlayerPrefs.SetInt("LevelPlayerHealth", value);
    }
    
    public int LevelPlayerSpeed
    {
        get => PlayerPrefs.GetInt("LevelPlayerSpeed", 0);
        set => PlayerPrefs.SetInt("LevelPlayerSpeed", value);
    }
    
    public int LevelPlayerRadius
    {
        get => PlayerPrefs.GetInt("LevelPlayerRadius", 0);
        set => PlayerPrefs.SetInt("LevelPlayerRadius", value);
    }

    public int GetLevelZoneHealth(int zoneNumber)
    {
        return PlayerPrefs.GetInt($"LevelZoneHealth{zoneNumber}", 0);
    }

    public void UpgradeLevelZoneHealth(int zoneNumber)
    {
        int level = GetLevelZoneHealth(zoneNumber) + 1;
        PlayerPrefs.SetInt($"LevelZoneHealth{zoneNumber}", level);
    }

    public int GetLevelZoneTrap(int zoneNumber)
    {
        return PlayerPrefs.GetInt($"LevelZoneTrap{zoneNumber}", 0);
    }

    public void UpgradeLevelZoneTrap(int zoneNumber)
    {
        int level = GetLevelZoneTrap(zoneNumber) + 1;
        PlayerPrefs.SetInt($"LevelZoneTrap{zoneNumber}", level);
    }

    public int GetLevelZoneAllies(int zoneNumber)
    {
        return PlayerPrefs.GetInt($"LevelZoneAllies{zoneNumber}", 0);
    }

    public void UpgradeLevelZoneAllies(int zoneNumber)
    {
        int level = GetLevelZoneAllies(zoneNumber) + 1;
        PlayerPrefs.SetInt($"LevelZoneAllies{zoneNumber}", level);
    }


    public int GetLevelWeapon(WeaponType weaponType)
    {
        return PlayerPrefs.GetInt($"LevelWeapon{weaponType}", 0);
    }

    public void UpgradeLevelWeapon(WeaponType weaponType)
    {
        int level = GetLevelWeapon(weaponType) + 1;
        PlayerPrefs.SetInt($"LevelWeapon{weaponType}", level);
        Player.Instance.PlayerShooting.UpdateWeponByType(weaponType);
    }


    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();
    }

    public void UpdateButtons()
    {
        if (_isActive)
        {
            foreach (UIButtonUpgrade button in _buttonsList)
            {
                if (button.gameObject.activeSelf)
                {
                    button.UpdateInfo(GetUpgradeLevel(button.UpgradeType));
                }
            }
            _weaponPanel.UpdateWeaponsInfo();
        }
    }

    public void ShowButtons(UpgradeZoneType upgradeZoneType, EnemyZone zone)
    {
        if (_isActive)
        {
            return;
        }
        _playerPanel.gameObject.SetActive(false);
        _zonePanel.gameObject.SetActive(false);
        _weaponPanel.gameObject.SetActive(false);
        _zone = zone;
        _isActive = true;
        if (upgradeZoneType == UpgradeZoneType.UZT_Player)
        {
            _animator = _playerPanel;
            _playerPanel.gameObject.SetActive(true);
        }
        else if(upgradeZoneType == UpgradeZoneType.UZT_EnemyZone)
        {
            _animator = _zonePanel;
            _zonePanel.gameObject.SetActive(true);
        }
        else if(upgradeZoneType == UpgradeZoneType.UZT_Weapon)
        {
            _animator = _weaponPanel.Animator;
            _weaponPanel.Activate();
            _weaponPanel.gameObject.SetActive(true);
        }
        UpdateButtons();
        _animator.CrossFade("Show", 0);
    }

    public void HideButtons()
    {
        if (!_isActive)
        {
            return;
        }
        _zone = null;
        _isActive = false;
        if (_animator != null)
        {
            _animator.CrossFade("Hide", 0);
        }
    }

    public void UpgradeLevel(UpgradeType upgradeType)
    {
        if(upgradeType == UpgradeType.UT_PlayerHealth)
        {
            LevelPlayerHealth++;
        }
        else if(upgradeType == UpgradeType.UT_PlayerSpeed)
        {
            LevelPlayerSpeed++;
        }
        else if(upgradeType == UpgradeType.UT_PlayerRadius)
        {
            LevelPlayerRadius++;
        }
        else if (upgradeType == UpgradeType.UT_ZoneHealth)
        {
            if (_zone != null)
            {
                UpgradeLevelZoneHealth(_zone.NumberZone);
            }
        }
        else if (upgradeType == UpgradeType.UT_ZoneTrap)
        {
            if (_zone != null)
            {
                UpgradeLevelZoneTrap(_zone.NumberZone);
            }
        }
        else if (upgradeType == UpgradeType.UT_ZoneAllies)
        {
            if (_zone != null)
            {
                UpgradeLevelZoneAllies(_zone.NumberZone);
            }
        }
        if (_zone != null)
        {
            _zone.Update(upgradeType);
        }
        else
        {
            Player.Instance.Update(upgradeType);
        }
    }

    public int GetUpgradeLevel(UpgradeType upgradeType)
    {
        if(upgradeType == UpgradeType.UT_PlayerHealth)
        {
            return LevelPlayerHealth;
        }
        else if(upgradeType ==UpgradeType.UT_PlayerSpeed)
        {
            return LevelPlayerSpeed;
        }
        else if(upgradeType == UpgradeType.UT_PlayerRadius)
        {
            return LevelPlayerRadius;
        }
        else if(upgradeType == UpgradeType.UT_ZoneHealth)
        {
            if(_zone != null)
            {
                return GetLevelZoneHealth(_zone.NumberZone);
            }else
            {
                return 0;
            }
        }
        else if(upgradeType == UpgradeType.UT_ZoneTrap)
        {
            if (_zone != null)
            {
                return GetLevelZoneTrap(_zone.NumberZone);
            }
            else
            {
                return 0;
            }
        }
        else if(upgradeType == UpgradeType.UT_ZoneAllies)
        {
            if (_zone != null)
            {
                return GetLevelZoneAllies(_zone.NumberZone);
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    public float GetRatioWeaponDamage(WeaponType weaponType)
    {
        float ratio = 1;
        if(weaponType == WeaponType.WT_Gun)
        {
            ratio = ratioWeaponDamageGun;
        }
        else if(weaponType == WeaponType.WT_Uzi)
        {
            ratio = ratioWeaponDamageUzi;
        }
        else if(weaponType == WeaponType.WT_Shotgun)
        {
            ratio = ratioWeaponDamageShotGun;
        }
        else if(weaponType == WeaponType.WT_Riffle)
        {
            ratio = ratioWeaponDamageRiffle;
        }
        return ratio;
    }

    public float GetRatioWeaponFireRate(WeaponType weaponType)
    {
        float ratio = 0;
        if(weaponType == WeaponType.WT_Gun)
        {
            ratio = ratioWeaponFireRateGun;
        }
        else if(weaponType == WeaponType.WT_Uzi)
        {
            ratio = ratioWeaponFireRateUzi;
        }
        else if(weaponType == WeaponType.WT_Shotgun)
        {
            ratio = ratioWeaponFireRateShotGun;
        }
        else if(weaponType == WeaponType.WT_Riffle)
        {
            ratio = ratioWeaponFireRateRiffle;
        }
        return ratio;
    }

}
