using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    public static Level Instance { get; private set; }

    [SerializeField] private NavMeshSurface _navMeshPlayer;
    [SerializeField] private NavMeshSurface _navMeshEnemy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //BakeNavMesh();
    }

    public void BakeNavMesh()
    {
        if (_navMeshPlayer != null)
        {
            _navMeshPlayer.BuildNavMesh();
        }
        if (_navMeshEnemy != null)
        {
            _navMeshEnemy.BuildNavMesh();
        }
    }

}
