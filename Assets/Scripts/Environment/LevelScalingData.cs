using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Scaling")]
public class LevelScalingData : ScriptableObject
{
    [field: SerializeField] public Vector3 MaxScale {get; private set;}
    [field: SerializeField] public Vector3 MinScale {get; private set;}
    [field: SerializeField] public float MinTraversableY {get; private set;}
    [field: SerializeField] public float MaxTraversableY {get; private set;}
}
