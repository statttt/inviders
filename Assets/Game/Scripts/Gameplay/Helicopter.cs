using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    public static Helicopter Instance { get; private set; }

    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        Instance = this;
    }

    public void Fly()
    {
        _animator.Play("Fly", 0, 0);
    }

    public void ShowPlayer()
    {
        _player.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }
}
