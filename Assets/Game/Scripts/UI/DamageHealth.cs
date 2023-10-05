using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHealth : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Text _damageText;
    [SerializeField] private Color _colorPlayer;

    public void SetDirection(Vector3 direction,int damage, bool isPlayer = false)
    {
        if (isPlayer)
        {
            _damageText.color = _colorPlayer;
        }
        _damageText.text = damage.ToString();
        Vector2 directionMove = new Vector2(direction.x, direction.z);
        directionMove = Quaternion.AngleAxis(-45, Vector3.forward) * directionMove;
        Vector2 point = (Vector2)_rectTransform.localPosition + directionMove.normalized * 150;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(point, 1f));
        sequence.Join(transform.DOScale(Vector3.zero, 1f)).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
