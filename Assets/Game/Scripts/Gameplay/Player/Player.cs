using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerShooting _playerShooting;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private CoinsParent _coinsParent;
    [SerializeField] private List<Enemy> _enemysList = new List<Enemy>();
    [SerializeField] private Transform _targetForRotate;
    [SerializeField] private ParticleSystem _upgradeParticle;

    public bool IsDie { get; set; }

    private PlayerStateMachine _stateMachine = new PlayerStateMachine();

    private bool _isActive;

    public bool IsOnZone { get; set; }

    public PlayerAnimator PlayerAnimator { get { return _playerAnimator; } }

    public PlayerMovement PlayerMovement { get { return _playerMovement; } }

    public PlayerHealth PlayerHealth { get { return _playerHealth; } }

    public PlayerShooting PlayerShooting { get { return _playerShooting; } }

    public PlayerStateMachine StateMachine { get { return _stateMachine; } }

    public CoinsParent CoinsParent { get { return _coinsParent; } }

    public List<Enemy> EnemysList { get { return _enemysList; } }

    public Transform TargetForRotate { get { return _targetForRotate; } }


    public int LevelHealth
    {
        get => PlayerPrefs.GetInt("LevelHealth", 1);
        set => PlayerPrefs.SetInt("LevelHealth", value);
    }

    public int LevelSpeed
    {
        get => PlayerPrefs.GetInt("LevelSpeed", 1);
        set => PlayerPrefs.SetInt("LevelSpeed", value);
    }

    public Zone Zone { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CameraMovement.Instance.Init();
        StartUpdate();
        _stateMachine.Initialize(new IdlePlayerState(this, PlayerAnimator));
        _isActive = true;
        _coinsParent.Activate(this);
        _targetForRotate.parent = Level.Instance.transform;
    }

    public void Activate()
    {
        _isActive = true;
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.FixedUpdate();
    }

    private void LateUpdate()
    {
        FindEnemy();
        if (!IsDie)
        {
            transform.LookAt(TargetForRotate.position);
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemysList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemysList.Remove(enemy);
    }

    public void FindEnemy()
    {
        _playerShooting.Target = null;
        if (_enemysList.Count > 0)
        {
            float distance = float.MaxValue;
            foreach (Enemy enemy in _enemysList)
            {
                float newDistance = (transform.position - enemy.transform.position).sqrMagnitude;
                if(newDistance < distance)
                {
                    distance = newDistance;
                    _playerShooting.Target = enemy;
                }
            }
        }
    }

    public void Die()
    {
        _stateMachine.ChangeState(new DiePlayerState(this, PlayerAnimator));
    }

    public void Update(UpgradeType upgradeType)
    {
        if(upgradeType == UpgradeType.UT_PlayerHealth)
        {
            UpdateHealth();
        }
        else if(upgradeType == UpgradeType.UT_PlayerSpeed)
        {
            UpdateSpeed();
        }
        else if(upgradeType == UpgradeType.UT_PlayerRadius)
        {
            UpdateRadius();
        }
        _upgradeParticle.Play();
    }

    public void StartUpdate()
    {
        UpdateHealth();
        UpdateSpeed();
        UpdateRadius();
    }

    public void UpdateHealth()
    {
        _playerHealth.UpdateHealth();
    }

    public void UpdateSpeed()
    {
        _playerMovement.UpdateSpeed();
    }

    public void UpdateRadius()
    {
        _playerShooting.UpdateRadius();
    }

}
