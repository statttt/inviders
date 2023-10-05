using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;

    [SerializeField] private GameObject _confettiVfx;
    [SerializeField] private GameObject _camera;

    private GameObject _target;
    private Vector3 _offset;

    private void Awake()
    {
        Instance = this;
    }
    
    public void Init()
    {

        _target = Player.Instance.gameObject;
        if(_offset == Vector3.zero)
        {
            _offset = transform.position - _target.transform.position;
        }

        SetActiveConfetti(false);
    }


    private void LateUpdate()
    {
        if (_target)
        {
            transform.position = _target.transform.position + _offset;
        }
    }

    public void SetActiveConfetti(bool val)
    {
        _confettiVfx.SetActive(val);
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(IShake(duration, magnitude));
    }

    private IEnumerator IShake(float duration, float magnitude)
    {
        Vector3 originalPos = _camera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _camera.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _camera.transform.localPosition = originalPos;
    }
}
