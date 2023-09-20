using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCatStats",
    menuName = "Cat Stats", order = 0)]

public class CatStats : ScriptableObject
{
    public float CatSenseRange;
    public float CatAttackRange;
}
