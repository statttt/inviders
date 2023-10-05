using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    [SerializeField] private int _targetFramerate;
    [SerializeField] private string _gameEntrySceneName;
    [SerializeField] private float _loadingScreenTime;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = _targetFramerate;
        Screen.orientation = ScreenOrientation.Portrait;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(DealyLoadGameScene(_loadingScreenTime));
    }

    private IEnumerator DealyLoadGameScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(_gameEntrySceneName);
    }

}
