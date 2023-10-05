using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingJoystick : Joystick
{
    private Vector3 _defPos;

    private void Awake()
    {
        _defPos = background.transform.position;
    }

    protected override void Start()
    {
        base.Start();
        //background.gameObject.SetActive(false);
        SetAlpha(0.5f);
    }

    public override void Reset()
    {
        background.transform.position = _defPos;
        background.gameObject.SetActive(false);
        base.Reset();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        background.position = eventData.position;
        //background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        SetAlpha(1.0f);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        background.gameObject.SetActive(false);
        SetAlpha(0.5f);
        background.transform.position = _defPos;
    }

    private void SetAlpha(float val)
    {
        Color color = background.GetComponent<Image>().color;
        color.a = val;
        background.GetComponent<Image>().color = color;

        color = handle.GetComponent<Image>().color;
        color.a = val;
        handle.GetComponent<Image>().color = color;
    }
}