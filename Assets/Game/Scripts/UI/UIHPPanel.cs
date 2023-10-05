using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPPanel : UIElement
{
    public static UIHPPanel Instance { get; private set; }

    [SerializeField] private HealthBar _healthBarPrefab;
    
    private List<HealthBar> _healthBarList = new List<HealthBar>();

    private void Awake()
    {
        Instance = this;
    }

    public override void Show()
    {
        base.Show();
        foreach (HealthBar healthBar in _healthBarList)
        {
            if(healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
        }
        _healthBarList.Clear();
    }

    public HealthBar CreateHealthBar(int level = 0)
    {
        HealthBar healthBar = Instantiate(_healthBarPrefab, transform);
        healthBar.SetLevel(level);
        _healthBarList.Add(healthBar);
        return healthBar;
    }
}
