using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _backGround;
    [SerializeField] private Sprite _activeWeapon;
    [SerializeField] private Sprite _deactiveWeapon;
    [SerializeField] private Text _damageValueText;
    [SerializeField] private Text _damageNextValueText;
    [SerializeField] private Text _fireRateValueText;
    [SerializeField] private Text _fireRateNextValueText;
    [SerializeField] private RectTransform _damageInfo;
    [SerializeField] private RectTransform _fireRateInfo;
    [SerializeField] private int _priceBuyWeapon;
    [SerializeField] private Text _priceBuyWeaponText;
    [SerializeField] private GameObject _arrowNextDamage;
    [SerializeField] private GameObject _arrowNextFireRate;


    public WeaponType WeaponType { get { return _weaponType; } }

    [SerializeField] protected int _price;
    [SerializeField] protected int _currentPrice;
    [SerializeField] protected float _priceRatio;
    [SerializeField] protected Text _priceText;

    public bool IsBuyed
    {
        get => PlayerPrefs.GetInt($"IsBuyed{_weaponType}", 0) == 1;
        set => PlayerPrefs.SetInt($"IsBuyed{_weaponType}", value == true ? 1 : 0);
    }

    public void UpdateInfo(int level)
    {
        if (_weaponType == UIWeaponPanel.Instance.StartWeapon)
        {
            IsBuyed = true;
        }
        UpdateValue();
        if (IsBuyed)
        {
            _upgradeButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
            UpdatePrice(level);
        }
        else
        {
            _priceBuyWeaponText.text = _priceBuyWeapon.ToString();
            _upgradeButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);
        }
        UpdateInteractable();
    }

    public void UpdatePrice(int level)
    {
        _currentPrice = (int)(_price * Mathf.Pow(_priceRatio, level));
        _priceText.text = _currentPrice.ToString();
    }

    public void OnClickUpgradeButton()
    {
        if (_currentPrice <= GameManager.Instance.Coins)
        {
            UIUpgrade.Instance.UpgradeLevelWeapon(_weaponType);
            GameManager.Instance.RemoveCoins(_currentPrice);
            ChooseWeapon();
        }
    }

    public void UpdateInteractable()
    {
        Button button = _upgradeButton;
        float price = _currentPrice;
        if(!IsBuyed)
        {
            button = _buyButton;
            price = _priceBuyWeapon;
        }
        if (price > GameManager.Instance.Coins)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    internal void Active()
    {
        _backGround.sprite = _activeWeapon;
    }

    internal void Deactive()
    {
        _backGround.sprite = _deactiveWeapon;
    }

    public void ChooseWeapon()
    {
        if (IsBuyed)
        {
            UIWeaponPanel.Instance.SetWeapon(_weaponType);
        }
    }

    public void UpdateValue()
    {
        Weapon weapon = Player.Instance.PlayerShooting.GetWeponByType(_weaponType);
        if(weapon != null)
        {
            _damageValueText.text = weapon.GetDamageValue().ToString("0.0");
            _fireRateValueText.text = weapon.GetFireRateValue().ToString("0.0");
            if (IsBuyed)
            {
                _damageNextValueText.gameObject.SetActive(true);
                _fireRateNextValueText.gameObject.SetActive(true);
                _arrowNextDamage.SetActive(true);
                _arrowNextFireRate.SetActive(true);
                _damageNextValueText.text = $"+{weapon.GetNextDamageValuePercents()}%";
                _fireRateNextValueText.text = $"+{weapon.GetNextFireRateValuePercents()}%";
            }
            else
            {
                _arrowNextDamage.SetActive(false);
                _arrowNextFireRate.SetActive(false);
                _damageNextValueText.gameObject.SetActive(false);
                _fireRateNextValueText.gameObject.SetActive(false);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_damageInfo);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_fireRateInfo);
        }
    }

    public void OnClickBuyButton()
    {
        if (_priceBuyWeapon <= GameManager.Instance.Coins)
        {
            BuyWeapon();
            GameManager.Instance.RemoveCoins((int)_priceBuyWeapon);
            ChooseWeapon();
        }
    }

    public void BuyWeapon()
    {
        if (!IsBuyed)
        {
            IsBuyed = true;
            UpdateInfo(0);
        }
    }
}
