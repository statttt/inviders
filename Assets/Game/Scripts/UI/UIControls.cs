using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : UIElement
{
    public static UIControls Instance;

    [SerializeField] private Joystick _joystick;

    public Joystick Joystick { get { return _joystick; } }

    public Vector3 GetDir() { return _joystick.Direction; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();
        _joystick.Reset();
    }
}
