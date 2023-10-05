using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFog : MonoBehaviour
{
    private Vector3 _startScale;

    public void Activate()
    {
        _startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear);
    }

    public void Show()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(_startScale, 0.5f).SetEase(Ease.Linear);
    }
}
