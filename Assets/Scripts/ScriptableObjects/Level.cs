using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public int id;
    public Vector2 riderPos;
    // public Vector2 riderVelocity
    public Vector2[] rewards;
    public float xBoundLower;
    public float xBoundUpper;
    public float yBoundLower;
    public float yBoundUpper;
}
