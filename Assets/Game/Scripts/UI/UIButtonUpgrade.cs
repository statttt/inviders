using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonUpgrade : MonoBehaviour
{
    [SerializeField] protected UpgradeType _upgradeType;
    [SerializeField] protected Button _button;
    [SerializeField] protected int _price;
    [SerializeField] protected int _currentPrice;
    [SerializeField] protected float _priceLevelRatio;
    [SerializeField] protected float _priceRatio;
    [SerializeField] protected Text _priceText;
    [SerializeField] protected Text _levelText;

    public UpgradeType UpgradeType { get { return _upgradeType; } }

    public void UpdateInfo(int level)
    {
        UpdatePrice(level);
        UpdateInteractable();
    }

    public void UpdatePrice(int level)
    {
        float ratioZone = 1;
        if(UIUpgrade.Instance!= null && UIUpgrade.Instance.Zone != null)
        {
            if(_upgradeType == UpgradeType.UT_ZoneHealth ||
                _upgradeType == UpgradeType.UT_ZoneTrap || 
                _upgradeType == UpgradeType.UT_ZoneAllies)
            {
                ratioZone = Mathf.Pow(_priceLevelRatio, UIUpgrade.Instance.Zone.NumberZone - 1);
            }
        }
        _currentPrice = (int)(_price * Mathf.Pow(_priceRatio, level)* ratioZone);
        _priceText.text = _currentPrice.ToString();
        if(_levelText != null)
        {
            _levelText.text = $"LV {level + 1}";
        }
    }

    public void OnClickButton()
    {
        if (_currentPrice <= GameManager.Instance.Coins)
        {
            UIUpgrade.Instance.UpgradeLevel(_upgradeType);
            GameManager.Instance.RemoveCoins(_currentPrice);
        }
    }

    public void UpdateInteractable()
    {
        if(_currentPrice > GameManager.Instance.Coins)
        {
            _button.interactable = false;
        }
        else
        {
            _button.interactable = true;
        }
    }
}
