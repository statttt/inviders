using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField] private float _timeForShow;
    [SerializeField] private List<Animator> _rootsAnimatorList = new List<Animator>();

    private Vector3 _startScale;

    private void Awake()
    {
        _startScale = transform.localScale;
    }


    public void ResetView()
    {
        transform.localScale = Vector3.zero;
        foreach (Animator root in _rootsAnimatorList)
        {
            root.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        transform.localScale = Vector3.zero;
        foreach (Animator root in _rootsAnimatorList)
        {
            root.Play("Show");
            root.gameObject.SetActive(true);
        }
        transform.DOScale(_startScale, _timeForShow).SetEase(Ease.Linear);
    }

    public void Hide()
    {
        foreach (Animator root in _rootsAnimatorList)
        {
            root.gameObject.SetActive(true);
            root.Play("Hide");
        }
        transform.localScale = _startScale;
        transform.DOScale(Vector3.zero, _timeForShow).SetEase(Ease.Linear);
    }

}
