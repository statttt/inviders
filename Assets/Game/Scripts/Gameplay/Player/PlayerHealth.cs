using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _startHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _offsetHealthbarPosition;
    [SerializeField] private float _speedReturnHealth;

    private float _health;

    private HealthBar _healthBar;

    private bool _isActive;
    
    public void UpdateHealth()
    {
        float ratio = (float)Math.Pow(UIUpgrade.Instance.ratioHealthPlayer, UIUpgrade.Instance.LevelPlayerHealth);
        _health = _startHealth * ratio;
        _currentHealth = _health;
        _healthBar = UIHPPanel.Instance.CreateHealthBar();
        if (_healthBar != null)
        {
            _healthBar.SetFillAmount(1.0f, -transform.forward,0, true);
        }
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        Destroy(_healthBar.gameObject);
        _isActive = false;
    }

    private void LateUpdate()
    {
        if(_healthBar != null)
        {
            SetHealthbarPosition();
        }
    }

    public void GetDamage(float damage)
    {
        if(_currentHealth <= 0)
        {
            return;
        }
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            _player.Die();
        }
        if (_healthBar != null)
        {
            _healthBar.SetFillAmount(_currentHealth / _health, -transform.forward, (int)damage, true);
        }

    }

    public void SetHealthbarPosition()
    {
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * _offsetHealthbarPosition);
    }

    public void ReturnHealth()
    {
        if(_currentHealth < _health)
        {
            _currentHealth = Mathf.Lerp(_currentHealth, _currentHealth + 10f, _speedReturnHealth);
        }
        else
        {
            _currentHealth = _health;
        }
        _healthBar.SetFillAmount(_currentHealth / _health, -transform.forward, 0, true);
    }

}
