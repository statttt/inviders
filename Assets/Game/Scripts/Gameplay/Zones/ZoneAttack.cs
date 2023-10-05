using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    public static ZoneAttack Instance;

    [SerializeField] private List<EnemyZone> _zonesList = new List<EnemyZone>();
    [SerializeField] private EnemyZone _zoneForAttack;
    [SerializeField] private float _delayBetweenStartAttack;
    [SerializeField] private float _timer;
    [SerializeField] private bool _isActiveTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ActivateTimer();
    }

    private void Update()
    {
        if(_isActiveTimer)
        {
            _timer += Time.deltaTime;
            UITimerBetweenAttack.Instance.UpdateTimer(_timer);
            if(_timer >= _delayBetweenStartAttack )
            {
                StartAttack();
                _timer = 0;
            }
        }
    }


    public void ActivateTimer()
    {
        if(GetRandomZoneForAttack() != null)
        {
            if (!_isActiveTimer)
            {
                _timer = 0;
                _isActiveTimer = true;
                UITimerBetweenAttack.Instance.ShowTimerInfo(_delayBetweenStartAttack);
            }
        }
        else
        {
            UITimerBetweenAttack.Instance.HideTimer();
        }
    }

    public void DeactivateTimer()
    {
        _timer = 0;
        _isActiveTimer = false;
        UITimerBetweenAttack.Instance.HideTimer();
    }


    public void StartAttack()
    {
        _zoneForAttack = GetRandomZoneForAttack();
        if(_zoneForAttack != null)
        {
            _zoneForAttack.AttackZone();
        }
        DeactivateTimer();
    }

    public EnemyZone GetRandomZoneForAttack()
    {
        List<EnemyZone> _zonesIsComplete = new List<EnemyZone>();
        foreach (EnemyZone zone in _zonesList)
        {
            if (zone.IsInAttack)
            {
                return null;
            }
            if (zone.IsComplete)
            {
                _zonesIsComplete.Add(zone);
            }
        }
        if(_zonesIsComplete.Count > 0)
        {
            return _zonesIsComplete[Random.Range(0, _zonesIsComplete.Count)];
        }
        else
        {
            return null;
        }
    }
}
