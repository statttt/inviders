using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseZone : Zone
{
    [SerializeField] private Helicopter _helicopter;
    [SerializeField] private List<UpgradeZone> _upgradePlayerZoneList = new List<UpgradeZone>();
    [SerializeField] private List<Transform> _gatesList = new List<Transform>();
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _heal;

    [SerializeField] private int _maxEnemyCount;

    protected override void Start()
    {
        base.Start();
        foreach (UpgradeZone zone in _upgradePlayerZoneList)
        {
            zone.gameObject.SetActive(false);
        }
        _heal.SetActive(false);
        if (!IsBaseComplete)
        {
            _maxEnemyCount = _enemyList.Count;
            foreach (Enemy enemy in _enemyList)
            {
                enemy.gameObject.SetActive(true);
                enemy.SetParameters(this, 1);
            }
            _helicopter.Fly();
            ShowSmog();
        }
        else
        {
            foreach (UpgradeZone zone in _upgradePlayerZoneList)
            {
                zone.Show();
            }
            _heal.transform.localScale = Vector3.zero;
            _heal.SetActive(true);
            _heal.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
            _helicopter.ShowPlayer();
            _helicopter.gameObject.SetActive(false);
            foreach (Transform gate in _gatesList)
            {
                gate.localPosition = gate.localPosition - Vector3.up * 2;
            }
            //Level.Instance.BakeNavMesh();
        }
    }

    private void FixedUpdate()
    {
        if(_player && IsBaseComplete)
        {
            _player.PlayerHealth.ReturnHealth();
        }
    }

    public bool IsBaseComplete
    {
        get => PlayerPrefs.GetInt("IsBaseComplete", 0) == 1;
        set => PlayerPrefs.SetInt("IsBaseComplete", value == true ? 1 : 0);
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        base.RemoveEnemy(enemy); 
        if (UIEnemyProgressBar.Instance.Zone == this)
        {
            UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
        }
        if (_enemyList.Count <= 0)
        {
            IsBaseComplete = true;
            OpenGates();
        }
    }

    public void OpenGates()
    {
        HideSmog();
        UIEnemyProgressBar.Instance.HideProgressBar();
        foreach (UpgradeZone zone in _upgradePlayerZoneList)
        {
            zone.Show();
        }
        _heal.transform.localScale = Vector3.zero;
        _heal.SetActive(true);
        _heal.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            UIEnemyProgressBar.Instance.Zone = this; 
            UIEnemyProgressBar.Instance.HideProgressBar();
            if (!IsBaseComplete)
            {
                UIEnemyProgressBar.Instance.ShowProgressBar();
                UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
            }
            player.IsOnZone = true;
            _player = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            UIEnemyProgressBar.Instance.Zone = null;
            if (!IsBaseComplete)
            {
                UIEnemyProgressBar.Instance.HideProgressBar();
            }
            player.IsOnZone = false;
            _player = null;
        }
    }
}
