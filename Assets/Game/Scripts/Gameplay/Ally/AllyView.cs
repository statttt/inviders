using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyView : EnemyView
{
    [SerializeField] protected List<GameObject> _head = new List<GameObject>();

    public override void ShowClothes()
    {
        base.ShowClothes();
        int randomUp = Random.Range(0, _head.Count + 1);
        if (randomUp != _head.Count)
        {
            _head[randomUp].SetActive(true);
        }
    }
}
