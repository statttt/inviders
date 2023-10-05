using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrophyRoad : UIElement
{
    [System.Serializable]
    private class RoadLevelState
    {
        public GameObject _complete;
        public GameObject _current;
        public GameObject _next;
    }

    [SerializeField] private Animation _uniqueLevel;
    [SerializeField] private List<RoadLevelState> _listLevels;

    public override void Show()
    {
        Init();

        base.Show();
    }

    private void Init()
    {
        int num = GameManager.Instance.LevelNum % (_listLevels.Count + 1);

        for (int i = 0; i < _listLevels.Count; ++i)
        {
            _listLevels[i]._complete.SetActive(false);
            _listLevels[i]._current.SetActive(false);
            _listLevels[i]._next.SetActive(false);

            if (i < num)
            {
                _listLevels[i]._complete.SetActive(true);
            }
            else if (i == num)
            {
                _listLevels[i]._current.SetActive(true);
            }
            else
            {
                _listLevels[i]._next.SetActive(true);
            }
        }

        if (num == _listLevels.Count)
        {
            _uniqueLevel.enabled = true;
        }
        else
        {
            _uniqueLevel.enabled = false;
        }
    }
}
