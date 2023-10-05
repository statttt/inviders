using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsContainer : MonoBehaviour
{
    [SerializeField] private List<Level> _listTutors;
    [SerializeField] private List<Level> _listLevels;

    public Level GetLevelPrefab(int lvl)
    {
        if(lvl < _listTutors.Count)
        {
            return _listTutors[lvl];
        }

        if (_listLevels.Count > 0)
        {
            lvl -= _listTutors.Count;
            int lvlTmp = lvl % _listLevels.Count;
            return _listLevels[lvlTmp];
        }

        return null;
    }
}
