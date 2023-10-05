using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> _skinList = new List<SkinnedMeshRenderer>();
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private List<Ally> _allies = new List<Ally>();
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private EnemyRagdoll _enemyRagdoll;
    [SerializeField] private EnemyFriendTrigger _enemyFriendTrigger;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private CoinsGroup _coinsGroup;
    [SerializeField] private int _level;
    [SerializeField] private float _delayBetweenFollowPlayer;

    private float _timerFollowPlayer;
    public CapsuleCollider Collider { get; set; }

    public Zone Zone { get; set; }
    public bool IsDestroyer { get; set; }

    private EnemyStateMachine _stateMachine = new EnemyStateMachine();

    private bool _isActive;

    public bool IsHavePlayer { get; set; }

    public bool IsCanStopped { get; set; }

    public bool IsDie { get; set; }

    public bool IsInZone { get; set; }

    public EnemyAnimator EnemyAnimator { get { return _enemyAnimator; } }

    public EnemyMovement EnemyMovement { get { return _enemyMovement; } }

    public EnemyHealth EnemyHealth { get { return _enemyHealth; } }

    public EnemyFriendTrigger EnemyFriendTrigger { get { return _enemyFriendTrigger; } }

    public EnemyRagdoll EnemyRagdoll { get { return _enemyRagdoll; } }
    public EnemyAttack EnemyAttack { get { return _enemyAttack; } }

    public EnemyStateMachine StateMachine { get { return _stateMachine; } }

    public Transform TargetForDestroy { get; set; }

    public CoinsGroup CoinsGroup { get { return _coinsGroup; } }

    public int Level { get { return _level; } }

    private void Awake()
    {
        Collider = GetComponent<CapsuleCollider>();
    }

    public void Init()
    {
        _skin.gameObject.SetActive(false);
        _skin = _skinList[UnityEngine.Random.Range(0, _skinList.Count)];
        _skin.gameObject.SetActive(true);
        _stateMachine.Initialize(new IdleEnemyState(this, EnemyAnimator));
        _timerFollowPlayer = _delayBetweenFollowPlayer;
        IsCanStopped = true;
        EnemyMovement.ActivateAgnet();
    }

    public void Activate(Transform target)
    {
        if (!IsDestroyer)
        {
            IsCanStopped = false;
            _timerFollowPlayer = 0;
        }
        _isActive = true;
        _enemyMovement.isGoToHome = false;
        Run();
        _enemyMovement.Target = target;
        _enemyHealth.ShowHealthBar();
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();
        if (!IsCanStopped)
        {
            if (_timerFollowPlayer < _delayBetweenFollowPlayer)
            {
                _timerFollowPlayer += Time.deltaTime;
                if (_timerFollowPlayer >= _delayBetweenFollowPlayer)
                {
                    IsCanStopped = true;
                    if (!IsHavePlayer)
                    {
                        EnemyHealth.HideHealthBar();
                        _enemyMovement.BackToZone();
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        _stateMachine.CurrentState.LateUpdate();
    }

    internal void Idle()
    {
        if (IsDie)
        {
            return;
        }
        _stateMachine.ChangeState(new IdleEnemyState(this, EnemyAnimator));
    }

    internal void Run()
    {
        if (IsDie)
        {
            return;
        }
        _stateMachine.ChangeState(new RunEnemyState(this, EnemyAnimator));
    }

    internal void Attack()
    {
        if (IsDie)
        {
            return;
        }
        _stateMachine.ChangeState(new EnemyAttackState(this, EnemyAnimator));
    }

    public void Die()
    {
        _stateMachine.ChangeState(new DieEnemyState(this, EnemyAnimator));
    }

    public void SetParameters(Zone zone, int level)
    {
        Init();
        IsInZone = true;
        Zone = zone;
        _level = level;
        EnemyHealth.SetHealth(_level);
        EnemyAttack.SetDamage(_level);
    }

    public void SetParametersForDestroyer(Zone zone, int level,Transform target)
    {
        Init();
        IsDestroyer = true;
        Zone = zone;
        _level = level;
        EnemyHealth.SetHealth(_level);
        EnemyAttack.SetDamage(_level);
        TargetForDestroy = target;
        Activate(TargetForDestroy);
    }

    public void Deactivate()
    {
        IsHavePlayer = false;
        EnemyAttack.Player = null;
        Destroy(EnemyAttack.EnemyAttackTrigger.gameObject);
        Destroy(EnemyFriendTrigger.gameObject);
    }

    public void RemoveAlly(Ally ally)
    {
        _allies.Remove(ally);
    }

    public void AddAlly(Ally ally)
    {
        _allies.Add(ally);
    }

    public void RemoveAllAllies()
    {
        foreach (Ally ally in _allies)
        {
            ally.RemoveEnemy(this);
        }
    }
}
