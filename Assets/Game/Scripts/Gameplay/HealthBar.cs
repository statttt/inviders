using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using System;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthLine;
    [SerializeField] private Slider _whiteHealthLine;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Sprite _healthBarSpritePlayer;
    [SerializeField] private Sprite _healthBarSpriteEnemy;
    [SerializeField] private GameObject _levelParent;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Animator _damageAniamtor;
    [SerializeField] private float _timeWhiteHPMove;
    [SerializeField] private DamageHealth _damageHealthPrefab;
    [SerializeField] private Transform _damageHealthparent;

    private Sequence sequence;

    public void SetFillAmount(float amount, Vector3 direction, int damage, bool isPlayer)
    {
        if(sequence != null)
        {
            sequence.Kill();
        }
        sequence = DOTween.Sequence();
        _healthLine.value = amount;
        sequence.Append(_whiteHealthLine.DOValue(_healthLine.value,_timeWhiteHPMove)).OnComplete(() =>
        {
            if(amount <= 0)
            {
                Destroy(gameObject);
            }
        });
        if (damage != 0)
        {
            DamageHealth damageHealth = Instantiate(_damageHealthPrefab, _damageHealthparent);
            damageHealth.SetDirection(direction, damage, isPlayer);
        }
    }

    public void SetLevel(int level)
    {
        if (level == 0)
        {
            _levelParent.SetActive(false);
            _healthBar.sprite = _healthBarSpritePlayer;
        }
        else
        {
            _healthBar.sprite = _healthBarSpriteEnemy;
            _levelText.text = level.ToString();
        }

    }

    internal void Deactivate()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
