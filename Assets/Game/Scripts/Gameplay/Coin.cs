using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Coin : MonoBehaviour
{
    [SerializeField] private CoinsGroup _coinsGroup;
    [SerializeField] private float _timeMoveToPlayer;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _forceForward;
    [SerializeField] private float _forceLeft;
    [SerializeField] private float _forceUp;
    [SerializeField] private Rigidbody _rb;

    private Sequence _sequence;

    public void Activate()
    {
        _sequence.Kill();
        CoinsParent parent = Player.Instance.CoinsParent;
        transform.parent = parent.transform;
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(parent.CoinMoveEndPoint.localPosition, _timeMoveToPlayer));
    }

    private void FixedUpdate()
    {
        if(_speedRotate > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, _speedRotate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            if (!_coinsGroup.IsGetMoney)
            {
                GameManager.Instance.AddCoins(_coinsGroup.Cost);
                _coinsGroup.IsGetMoney = true;
            }
            _sequence.Kill();
            Destroy(gameObject);
        }
    }

    internal void AddForce(CoinsGroup coinsGroup)
    {
        gameObject.SetActive(true);
        _coinsGroup = coinsGroup;
        Transform transform = coinsGroup.transform;
        Vector3 direcion = transform.forward * Random.Range(-_forceForward, _forceForward) +
            transform.up * _forceUp + transform.right * Random.Range(-_forceLeft, _forceLeft);
        _rb.AddForce(direcion, ForceMode.Impulse);
    }
}
