using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using System;

public enum UIState { Start, ActionPhase, Lose, Finish }

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private List<UIStateViewData> _stateViews;

    private List<UIElement> _uiElements;
    private List<UIElement> _stateDisabledUiElements;

    private UIStateViewData _requestedViewData;

    private UIState _currentState;

    public event Action OnStartGame;

    public UIState GetCurrentState() { return _currentState; }

    private void Awake()
    {
        Instance = this;

        InitUIElements();
    }

    private void InitUIElements()
    {
        _uiElements = new List<UIElement>();

        for (int i = 0; i < _stateViews.Count; ++i)
        {
            for(int j = 0; j < _stateViews[i].activeElements.Length; ++j)
            {
                UIElement tmpElement = _stateViews[i].activeElements[j];
                if (!_uiElements.Contains(tmpElement))
                {
                    _uiElements.Add(tmpElement);
                }
            }
        }

        _currentState = UIState.ActionPhase;
    }

    public void ChangeState(UIState state)
    {
        _requestedViewData = _stateViews.Find((v) => state == v.uIState);
        _stateDisabledUiElements = new List<UIElement>(_uiElements);

        for (int i = 0; i < _requestedViewData.activeElements.Length; i++)
        {
            _requestedViewData.activeElements[i].Show();
            _stateDisabledUiElements.Remove(_requestedViewData.activeElements[i]);
        }

        for (int i = 0; i < _stateDisabledUiElements.Count; i++)
        {
            _stateDisabledUiElements[i].Hide();
        }

        _currentState = state;
    }

    private void OnValidate()
    {
        for (int i = 0; i < _stateViews.Count; i++)
        {
            _requestedViewData = _stateViews[i];

            _requestedViewData.title = _stateViews[i].uIState.ToString();

            _stateViews[i] = _requestedViewData;
        }
    }

    public void OnStartGameClick()
    {
        ChangeState(UIState.ActionPhase);

        GameManager.Instance.OnLevelStarted();

        OnStartGame?.Invoke();
    }

    public void OnTestWinLoseClick(bool isWin)
    {
        GameManager.Instance.OnLevelFinished(isWin);
    }

    public void OnEndScreenBtnClick(bool isWin)
    {
        GameManager.Instance.PlayHaptic(HapticTypes.MediumImpact);

        GameManager.Instance.Init();
    }

    public void RestartGame()
    {

    }
}

[System.Serializable]
public struct UIStateViewData
{
    [HideInInspector] public string title;

    public UIState uIState;
    public UIElement[] activeElements;
}
