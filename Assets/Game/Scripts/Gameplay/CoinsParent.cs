using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsParent : MonoBehaviour
{
    [SerializeField] private Transform _coinMoveEndPoint;
    [SerializeField] private Player _player;

    public Transform CoinMoveEndPoint { get { return _coinMoveEndPoint; } }

    internal void Activate(Player player)
    {
        transform.parent = Level.Instance.transform;
        _player = player;
    }

    private void LateUpdate()
    {
        if(_player != null)
        {
            transform.position = _player.transform.position;
        }
    }
}
