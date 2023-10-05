using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class UIWinScreen : UIElement
{
    public static UIWinScreen Instance;

    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _btnCollect;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _needPos;
    [SerializeField] private Vector2 _coinsSpeeds;

    private class FlyCoin
    {
        public FlyCoin(GameObject coin, float speed, Vector3 customPos)
        {
            _coin = coin;
            _speed = speed;
            _customPos = customPos;
            _goToCustomPos = true;
        }

        public GameObject _coin;
        public float _speed;
        public Vector3 _customPos;
        public bool _goToCustomPos;
    }

    private List<FlyCoin> _listCoins;

    private bool _isCoinsFly;

    private void Awake()
    {
        Instance = this;

        _isCoinsFly = false;
    }

    public override void Show()
    {
        base.Show();

        _animator.Play("ShowPanel", 0, 0.0f);

        _btnCollect.SetActive(false);

        CameraMovement.Instance.SetActiveConfetti(true);

        StartCoroutine(SpawnCoins(1.25f));
    }

    private IEnumerator SpawnCoins(float time)
    {
        yield return new WaitForSeconds(time);
        _listCoins = new List<FlyCoin>();

        int cnt = Random.Range(10, 15);

        for(int i = 0; i < cnt; ++i)
        {
            GameObject coin = Instantiate(_coinPrefab, _canvas.transform);

            float randX = Screen.width * 0.5f + Random.Range(-100, 100);
            float randY = Screen.height * 0.5f + Random.Range(-100, 100);

            Vector3 customPos = new Vector3(randX, randY, 0);

            float speed = Random.Range(_coinsSpeeds.x, _coinsSpeeds.y);

            _listCoins.Add(new FlyCoin(coin, speed, customPos));
        }

        _isCoinsFly = true;
    }

    private void FixedUpdate()
    {
        if(_isCoinsFly)
        {
            for (int i = 0; i < _listCoins.Count; ++i)
            {
                float speed = _listCoins[i]._speed;
                Vector3 needPos = _needPos.position;
                if (_listCoins[i]._goToCustomPos)
                {
                    needPos = _listCoins[i]._customPos;
                    speed *= 0.35f;
                }

                Vector3 dir = needPos - _listCoins[i]._coin.transform.position;
                float dist = dir.magnitude;

                float speedInDt = speed * Time.fixedDeltaTime;
                if (dist < speedInDt)
                {
                    if(_listCoins[i]._goToCustomPos)
                    {
                        _listCoins[i]._goToCustomPos = false;
                    }
                    else
                    {
                        Destroy(_listCoins[i]._coin);
                        _listCoins.RemoveAt(i);

                        GameManager.Instance.PlayHaptic(HapticTypes.LightImpact);
                    }
                }
                else
                {
                    _listCoins[i]._coin.transform.position += dir.normalized * speedInDt;
                }
            }

            if(_listCoins.Count == 0)
            {
                _isCoinsFly = false;

                _btnCollect.SetActive(true);

                _btnCollect.GetComponent<Animator>().Play("ShowPanel", 0, 0.0f);

                UICoins.Instance.UpdateText();
            }
        }
    }
}
