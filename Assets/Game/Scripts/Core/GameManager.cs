using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using Tabtale.TTPlugins;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelsContainer _levelsContainer;

    [SerializeField] private int _coinsAddByLvl;

    private Level _curLevel;
    private bool _isInAP;

    public int LevelNum
    {
        get => PlayerPrefs.GetInt("Level", 0);
        set => PlayerPrefs.SetInt("Level", value);
    }

    public bool HapticEnabled
    {
        get => PlayerPrefs.GetInt("HapticEnabled", 1) == 1;
        set => PlayerPrefs.SetInt("HapticEnabled", value == true ? 1 : 0);
    }

    public int Coins
    {
        get => PlayerPrefs.GetInt("Coins", 0);
        set => PlayerPrefs.SetInt("Coins", value);
    }

    public int UpgradeElement
    {
        get => PlayerPrefs.GetInt("UpgradeElement", 0);
        set => PlayerPrefs.SetInt("UpgradeElement", value);
    }

    public int GetCoinsAddByLvl() { return _coinsAddByLvl; }

    private void Awake()
    {
        TTPCore.Setup();

        Instance = this;

        _isInAP = false;
        _curLevel = null;

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddCoins(1000);
        }
    }

    public void Init()
    {
        if(_curLevel != null)
        {
            Destroy(_curLevel.gameObject);
        }

        Level level = _levelsContainer.GetLevelPrefab(LevelNum);
        if(level)
        {
            _curLevel = Instantiate(level);
        }

        UIManager.Instance.OnStartGameClick();
    }

    public void OnLevelStarted()
    {
        _isInAP = true;
        PlayHaptic(HapticTypes.MediumImpact);
    }

    public void OnLevelFinished(bool success)
    {
        if(success)
        {
            UIManager.Instance.ChangeState(UIState.Finish);

            LevelNum++;

            Coins += _coinsAddByLvl;
        }
        else
        {
            UIManager.Instance.ChangeState(UIState.Lose);
        }
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        UICoins.Instance.UpdateText();
    }

    public void RemoveCoins(int coins)
    {
        if(coins <= Coins)
        {
            Coins -= coins;
            UICoins.Instance.UpdateText();
        }
    }

    public void PlayHaptic(HapticTypes type)
    {
        if (HapticEnabled)
        {
            MMVibrationManager.Haptic(type);
        }
    }

}
