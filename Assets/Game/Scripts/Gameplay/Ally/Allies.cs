using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allies : MonoBehaviour
{
    [SerializeField] private EnemyZone _zone;
    [SerializeField] private Ally _allyPrefab;
    [SerializeField] private List<Transform> _alliesPointList = new List<Transform>();
    [SerializeField] private List<Ally> _alliesList = new List<Ally>();
    [SerializeField] private float _damage;
    [SerializeField] private float _currentDamage;
    [SerializeField] private float _delayBetweenShoot;
    [SerializeField] private float _currentDelayBetweenShoot;

    private void Start()
    {
        
    }

    public void CreateAllies()
    {
        for (int i = 0; i < _alliesPointList.Count; i++)
        {
            Ally ally = Instantiate(_allyPrefab);
            ally.transform.parent = transform;
            ally.transform.localPosition = _alliesPointList[i].transform.localPosition;
            ally.transform.rotation = Quaternion.LookRotation(ally.transform.position - transform.position, Vector3.up);
            _alliesList.Add(ally);
        }
        UpdateAllies();
    }

    public void UpdateAllies()
    {
        if(_alliesList.Count > 0)
        {
            float ratioDamage = (float)Math.Pow(UIUpgrade.Instance.ratioAlliesDamageZone, UIUpgrade.Instance.GetLevelZoneAllies(_zone.NumberZone));
            _currentDamage = _damage * ratioDamage;
            float ratioFireRate = (float)Math.Pow(UIUpgrade.Instance.ratioAlliesFireRateZone, UIUpgrade.Instance.GetLevelZoneAllies(_zone.NumberZone));
            _currentDelayBetweenShoot = _delayBetweenShoot * ratioFireRate;
            foreach (Ally ally in _alliesList)
            {
                ally.AllyShooting.SetParameters(_currentDamage, _currentDelayBetweenShoot);
                ally.gameObject.SetActive(UIUpgrade.Instance.GetLevelZoneAllies(_zone.NumberZone) > 0);
            }
        }
    }

    public void Activate(bool isFear)
    {
        foreach (Ally ally in _alliesList)
        {
            ally.IsFear = isFear;
            ally.AllyAnimator.SetIsFear(isFear);
        }
    }

}
