using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private Animator _skinAnimator;
    [SerializeField] private Transform _bodyView;
    [SerializeField] private float _startHealth;
    [SerializeField] private float _hitViewScaleValue;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _hitColor;
    [SerializeField] private Color _dieColor;
    [SerializeField] private float _offsetHealthbarPosition;

    private Sequence _sequence;

    private float _currentHealth;

    private HealthBar _healthBar;

    private void Start()
    {
        _currentHealth = _startHealth;
        _healthBar = UIHPPanel.Instance.CreateHealthBar(_enemy.Level);
        if (_healthBar != null)
        {
            _healthBar.SetFillAmount(1.0f, -transform.forward, 0, false);
        }
        if (_enemy.IsDestroyer)
        {
            ShowHealthBar();
        }
        else
        {
            HideHealthBar();
        }
    }

    public void Activate()
    {

    }

    public void Deactivate()
    {
        if(_healthBar!= null)
        {
            _healthBar.Deactivate();
        }
    }

    private void LateUpdate()
    {
        if (_healthBar != null)
        {
            SetHealthbarPosition();
        }
    }

    public void GetDamage(float damage, Vector3 direction)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _enemy.Die();
            DieChangeColor();
        }
        else
        {
            HitChangeColor();
        }
        if (_healthBar != null)
        {
            _healthBar.SetFillAmount(_currentHealth / _startHealth, direction, (int)damage, false);
        }

    }

    public void SetHealthbarPosition()
    {
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * _offsetHealthbarPosition);
    }

    public void HitChangeColor()
    {
        _skinAnimator.Rebind();
        _skinAnimator.Play("BodyColorChange");
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        //_sequence.Append(_skin.material.DOColor(_hitColor, .1f).SetEase(Ease.Linear));
        _sequence.Append(_bodyView.DOScale(Vector3.one * _hitViewScaleValue, .1f).SetEase(Ease.Linear));
        //_sequence.Append(_skin.material.DOColor(_baseColor, .1f).SetEase(Ease.Linear));
        _sequence.Append(_bodyView.DOScale(Vector3.one, .1f).SetEase(Ease.Linear));
    }

    public void DieChangeColor()
    {
        //_sequence.Kill();
        //_sequence = DOTween.Sequence();
        //_sequence.Append(_skin.material.DOColor(_dieColor, .3f).SetEase(Ease.Linear));
        _skinAnimator.Play("DieColor");
    }

    internal void SetHealth(int level)
    {
        _startHealth *= level;
        _currentHealth = _startHealth;
    }

    public void ShowHealthBar()
    {
        if(_healthBar != null)
        {
            _healthBar.gameObject.SetActive(true);
        }
    }

    public void HideHealthBar()
    {
        if(_healthBar != null)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }
}
