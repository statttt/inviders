using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _shooes = new List<GameObject>();
    [SerializeField] protected List<GameObject> _down = new List<GameObject>();
    [SerializeField] protected List<GameObject> _up = new List<GameObject>();
    [SerializeField] protected GameObject _clock;

    public void Start()
    {
        ShowClothes();
    }

    public virtual void ShowClothes()
    {
        int randomClock = Random.Range(0, 1);
        if(randomClock == 1)
        {
            _clock.gameObject.SetActive(true);
        }
        int randomShoes = Random.Range(0, _shooes.Count + 1);
        if(randomShoes != _shooes.Count)
        {
            _shooes[randomShoes].SetActive(true);
        }
        int randomDown = Random.Range(0, _down.Count + 1);
        if(randomDown != _down.Count)
        {
            _down[randomDown].SetActive(true);
        }
        int randomUp = Random.Range(0, _up.Count + 1);
        if(randomUp != _up.Count)
        {
            _up[randomUp].SetActive(true);
        }
    }
}
