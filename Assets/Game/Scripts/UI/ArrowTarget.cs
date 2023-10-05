using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTarget : MonoBehaviour
{
    [SerializeField] private RectTransform _marker;
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageZombie;

    public RectTransform Marker { get { return _marker; } }
    public Image Image { get { return _image; } }

    public bool IsActive { get; set; }
    public bool IsHasPlayer { get; set; }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if(_imageZombie != null)
        {
            _imageZombie.rectTransform.eulerAngles = Vector3.zero;
        }
    }
}
