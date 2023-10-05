using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGroup : MonoBehaviour
{
    [SerializeField] private bool _isGetMoney;
    [SerializeField] private int _startCost;
    [SerializeField] private int _cost;
    [SerializeField] private int _ratioCost;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private int _maxCoinCount;
    [SerializeField] private float _delayBetweenMoveToPlayer;
    [SerializeField] private float _delayBetweenMoveNextCoin;
    [SerializeField] private float _delayBetweenSpawnNextCoin;
    [SerializeField] private List<Coin> _coinList = new List<Coin>();

    public bool IsGetMoney 
    { 
        get 
        { 
            return _isGetMoney; 
        } 
        set 
        { 
            _isGetMoney = value; 
        }
    }

    public int Cost { get { return _cost; } }

    private void Start()
    {
        for (int i = 0; i < _maxCoinCount; i++)
        {
            Coin coin = Instantiate(_coinPrefab, transform);
            coin.gameObject.SetActive(false);
            _coinList.Add(coin);
        }
    }

    public void Activate(int level)
    {
        _cost = (int)(_startCost * Math.Pow(_ratioCost, level - 1));
        StartCoroutine(ISpawnCoin());
    }

    private IEnumerator IMoveToPlayer()
    {
        yield return new WaitForSeconds(_delayBetweenMoveToPlayer);
        foreach (Coin coin in _coinList)
        {
            if(coin != null)
            {
                coin.Activate();
                yield return new WaitForSeconds(_delayBetweenMoveNextCoin);
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator ISpawnCoin()
    {
        foreach (Coin coin in _coinList)
        {
            coin.AddForce(this);
            yield return new WaitForSeconds(_delayBetweenSpawnNextCoin);
        }
        StartCoroutine(IMoveToPlayer());
    }

}
