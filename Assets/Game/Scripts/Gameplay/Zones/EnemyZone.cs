using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : Zone
{
    [SerializeField] private EnemySpawnerForAttack _enemyspawnerForAttack;
    [SerializeField] private UpgradeZone _upgradeZone;
    [SerializeField] private Allies _allies;
    [SerializeField] private List<EnemyZone> _friendZoneList = new List<EnemyZone>();
    [SerializeField] private List<Trap> _trapList = new List<Trap>();
    [SerializeField] private int _countEnemysForHelp;
    [SerializeField] private float _startHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _startDamage;
    [SerializeField] private float _currentDamage;
    [SerializeField] private int _numberZone;
    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private float _delayBetweenSpawnEnemy;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _notCompleteColor;
    [SerializeField] private Color _completeColor;
    [SerializeField] private Color _attackColor;
    [SerializeField] private List<GameObject> _environmentList = new List<GameObject>();
    [SerializeField] private float _timer;

    private float _health;
    private ArrowTarget _marker;

    public List<GameObject> EnvironmentList { get { return _environmentList; } }

    public int NumberZone { get { return _numberZone; } }
    public EnemySpawnerForAttack EnemySpawnerForAttack { get { return _enemyspawnerForAttack; } }

    public bool IsComplete
    {
        get => PlayerPrefs.GetInt($"IsComplete{_numberZone}", 0) == 1;
        set => PlayerPrefs.SetInt($"IsComplete{_numberZone}", value == true ? 1 : 0);
    }

    public bool IsInAttack
    {
        get => PlayerPrefs.GetInt($"IsInAttack{_numberZone}", 0) == 1;
        set => PlayerPrefs.SetInt($"IsInAttack{_numberZone}", value == true ? 1 : 0);
    }

    protected override void Start()
    {
        base.Start();
        UpdateInfo();
        if(_allies != null)
        {
            _allies.CreateAllies();
        }
        if (!IsComplete)
        {
            _timer = _delayBetweenSpawnEnemy;
            for (int i = 0; i < _maxEnemyCount; i++)
            {
                Enemy enemy = EnemySpawner.CreateEnemy(this, _numberZone);
                AddEnemy(enemy);
            }
            _meshRenderer.material.color = _notCompleteColor;
            _upgradeZone.gameObject.SetActive(false);
            foreach (Trap trap in _trapList)
            {
                trap.gameObject.SetActive(false);
            }

            _enemySpawner.gameObject.SetActive(true);
            ShowSmog();
            if(_allies != null)
            {
                _allies.Activate(true);
            }
        }
        else
        {
            if (IsInAttack)
            {
                AttackZone();
            }
            else
            {
                _enemySpawner.gameObject.SetActive(false);
                _meshRenderer.material.color = _completeColor;
                _upgradeZone.gameObject.SetActive(true);
                foreach (Trap trap in _trapList)
                {
                    trap.gameObject.SetActive(UIUpgrade.Instance.GetLevelZoneTrap(NumberZone) > 0);
                }
            }
        }
        _timer = _delayBetweenSpawnEnemy;
        IndicatorsManager.Instance.AddTarget(this);
    }

    private void Update()
    {
        if (IsComplete)
        {
            return;
        }
        if(_timer < _delayBetweenSpawnEnemy)
        {
            _timer += Time.deltaTime;
        }
        if(_enemyList.Count < _maxEnemyCount && _timer >= _delayBetweenSpawnEnemy)
        {
            Enemy enemy = EnemySpawner.CreateEnemy(this,_numberZone);
            AddEnemy(enemy);
            _timer = 0;
        }
    }

    public override void AddEnemy(Enemy enemy)
    {
        base.AddEnemy(enemy);
        if(UIEnemyProgressBar.Instance.Zone == this )
        {
            UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
        }
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        base.RemoveEnemy(enemy);
        if (_timer >= _delayBetweenSpawnEnemy)
        {
            _timer = 0;
        }
        if(UIEnemyProgressBar.Instance.Zone == this)
        {
            UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
        }
        if(_enemyList.Count <= 0)
        {
            CompleteZone();
        }
    }
    
    public override void AddAttackEnemy(Enemy enemy)
    {
        base.AddAttackEnemy(enemy);
    }

    public override void RemoveAttackEnemy(Enemy enemy)
    {
        base.RemoveAttackEnemy(enemy);
        if(_enemyAttackList.Count <= 0)
        {
            CompleteZone();
        }
    }

    public void CompleteZone()
    {
        if (_marker != null)
        {
            _marker.Deactivate();
        }
        if(!IsInAttack)
        {
            HideSmog();
        }
        _allies.Activate(false);
        _upgradeZone.Show();
        foreach (Trap trap in _trapList)
        {
            trap.gameObject.SetActive(UIUpgrade.Instance.GetLevelZoneTrap(NumberZone) > 0);
        }
        _enemySpawner.gameObject.SetActive(false);
        IsInAttack = false;
        IsComplete = true;
        _meshRenderer.material.color = _completeColor;
        if (UIEnemyProgressBar.Instance.Zone == this)
        {
            UIEnemyProgressBar.Instance.HideProgressBar();
        }
        ZoneAttack.Instance.ActivateTimer();
        UIZoneAttackProgressBar.Instance.HideProgressBar();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemy.Zone == this)
            {
                enemy.IsInZone = true;
            }
        }
        if (other.TryGetComponent(out Player player))
        {
            player.Zone = this;
            if (IsInAttack)
            {
                if(_marker != null && _marker.IsActive)
                {
                    _marker.IsHasPlayer = true;
                    _marker.gameObject.SetActive(false);
                }
                UIZoneAttackProgressBar.Instance.Zone = this;
                UIZoneAttackProgressBar.Instance.ShowProgressBar();
                UIZoneAttackProgressBar.Instance.SetValue(1f - _currentHealth / _health);
            }
            else if (IsComplete)
            {
                return;
            }
            else
            {
                UIEnemyProgressBar.Instance.Zone = this;
                UIEnemyProgressBar.Instance.ShowProgressBar();
                UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if(enemy.Zone == this)
            {
                enemy.IsInZone = false;
            }
        }

        if (other.TryGetComponent(out Player player))
        {
            player.Zone = null;
            if (IsInAttack)
            {
                if (_marker != null && _marker.IsActive)
                {
                    _marker.IsHasPlayer = false;
                    _marker.gameObject.SetActive(true);
                }
                UIEnemyProgressBar.Instance.Zone = null;
                UIEnemyProgressBar.Instance.HideProgressBar();
            }
            else if (IsComplete)
            {
                return;
            }
            else
            {
                UIEnemyProgressBar.Instance.Zone = null;
                UIEnemyProgressBar.Instance.HideProgressBar();
            }
        }
        
    }

    public void AttackZone()
    {
        if(_marker != null)
        {
            _marker.Activate();
        }
        _enemySpawner.gameObject.SetActive(true);
        foreach (Trap trap in _trapList)
        {
            trap.gameObject.SetActive(UIUpgrade.Instance.GetLevelZoneTrap(NumberZone) > 0);
        }
        _upgradeZone.Hide();
        _currentHealth = _health;
        IsInAttack = true;
        _meshRenderer.material.color = _attackColor;
        _enemyspawnerForAttack.CreateForAttack();
        foreach (EnemyZone zone in _friendZoneList)
        {
            if (!zone.IsComplete && !zone.IsInAttack)
            {
                for (int i = 0; i < _countEnemysForHelp; i++)
                {
                    zone.EnemySpawnerForAttack.CreateEnemyForAttackHelp(Random.Range(NumberZone, NumberZone + 2), this);
                }
            }
        }
        Zone playerZone = Player.Instance.Zone;
        if (playerZone != null && playerZone == this)
        {
            UIZoneAttackProgressBar.Instance.Zone = this;
            UIZoneAttackProgressBar.Instance.ShowProgressBar();
            UIZoneAttackProgressBar.Instance.SetValue(1f - _currentHealth / _health);
        }
    }

    internal void GetDamage(float damage)
    {
        if(_currentHealth < 0 || !IsInAttack)
        {
            return;
        }
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_marker != null)
            {
                _marker.Deactivate();
            }
            ShowSmog();
            _allies.Activate(true);
            UIZoneAttackProgressBar.Instance.HideProgressBar();
            _currentHealth = 0;
            UIZoneAttackProgressBar.Instance.SetValue(1f - _currentHealth / _health);
            int maxCount = _maxEnemyCount;
            foreach (Enemy enemy in _enemyAttackList)
            {
                enemy.IsDestroyer = false;
                if (maxCount > 0)
                {
                    _enemyList.Add(enemy);
                    enemy.EnemyMovement.BackToZone();
                    maxCount--;
                }
                else
                {
                    enemy.Deactivate();
                    enemy.EnemyMovement.BackToNest();
                    Player.Instance.RemoveEnemy(enemy);
                }
            }
            _enemyAttackList.Clear();
            IsComplete = false;
            IsInAttack = false;
            _meshRenderer.material.color = _notCompleteColor;
            _upgradeZone.Hide();
            foreach (Trap trap in _trapList)
            {
                trap.gameObject.SetActive(false);
            }
            ZoneAttack.Instance.ActivateTimer();
            Zone playerZone = Player.Instance.Zone;
            if (playerZone != null && playerZone == this)
            {
                UIEnemyProgressBar.Instance.Zone = this;
                UIEnemyProgressBar.Instance.ShowProgressBar();
                UIEnemyProgressBar.Instance.SetValue((float)_enemyList.Count / _maxEnemyCount);
            }
        }
        if (UIZoneAttackProgressBar.Instance.Zone == this)
        {
            UIZoneAttackProgressBar.Instance.SetValue(1f - _currentHealth / _health);
        }
    }

    public void UpdateInfo()
    {
        UpdateHealth();
        UpdateTrap();
        UpdateAllies();
    }

    public void Update(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.UT_ZoneHealth)
        {
            UpdateHealth();
        }
        else if (upgradeType == UpgradeType.UT_ZoneTrap)
        {
            UpdateTrap();
        }
        else if (upgradeType == UpgradeType.UT_ZoneAllies)
        {
            UpdateAllies();
        }
    }

    private void UpdateHealth()
    {
        float ratioHealth = (float)System.Math.Pow(UIUpgrade.Instance.ratioHealthZone, UIUpgrade.Instance.GetLevelZoneHealth(NumberZone));
        _health = _startHealth * ratioHealth;
        _currentHealth = _health;
    }

    private void UpdateAllies()
    {
        _allies.UpdateAllies();
    }

    private void UpdateTrap()
    {
        float ratioTrap = (float)System.Math.Pow(UIUpgrade.Instance.ratioTrapZone, UIUpgrade.Instance.GetLevelZoneTrap(NumberZone));
        _currentDamage = _startDamage * ratioTrap;
        foreach (Trap trap in _trapList)
        {
            trap.SetDamage(_currentDamage);
        }
        foreach (Trap trap in _trapList)
        {
            trap.gameObject.SetActive(UIUpgrade.Instance.GetLevelZoneTrap(NumberZone) > 0);
        }
    }



    public void SetMarker(ArrowTarget marker)
    {
        _marker = marker;
    }
}
