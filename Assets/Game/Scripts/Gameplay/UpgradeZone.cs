using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum UpgradeZoneType
{
    UZT_Player,
    UZT_EnemyZone,
    UZT_Weapon
}

public class UpgradeZone : MonoBehaviour
{
    [SerializeField] private EnemyZone _zone;
    [SerializeField] private UpgradeZoneType _upgradeZoneType;

    public void Show()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
    }

    public void Hide()
    {
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            UIUpgrade.Instance.ShowButtons(_upgradeZoneType, _zone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIUpgrade.Instance.HideButtons();
        }
    }

}
