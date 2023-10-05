using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BulletType
{
    Player,
    Enemy,
    Ally
}

[CreateAssetMenu(fileName = "NewBullet", menuName = "Bullets/NewBullet")]
public class BulletInfo : ScriptableObject
{
    public BulletType BulletType;
    public float Speed;
    public Bullet BulletPrefab;
    public float Damage;
}
