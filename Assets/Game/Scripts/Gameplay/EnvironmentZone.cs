using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshObstacle))]
public class EnvironmentZone : MonoBehaviour
{
    [SerializeField] private EnemyZone _zone;
    [SerializeField] private BoxCollider _colliderBox;
    [SerializeField] private CapsuleCollider _colliderSphere;
    [SerializeField] private NavMeshObstacle _obstacle;

    private void Start()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        _colliderBox = GetComponent<BoxCollider>();
        _colliderSphere = GetComponent<CapsuleCollider>();
        if(_obstacle != null)
        {
            _obstacle.carving = true;
        }
        if(_colliderBox != null)
        {
            _obstacle.shape = NavMeshObstacleShape.Box;
            _obstacle.center = _colliderBox.center;
            _obstacle.size = _colliderBox.size;
        }else if (_colliderSphere != null)
        {
            _obstacle.shape = NavMeshObstacleShape.Capsule;
            _obstacle.center = _colliderSphere.center;
            _obstacle.radius = _colliderSphere.radius;
            _obstacle.height = _colliderSphere.height;
        }
    }

    public EnemyZone Zone { get { return _zone; } }
}
