using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritOptimizer : MonoBehaviour
{
    public float normalDamage;
    [Space]
    public float critRate;
    public float critDamage;

    private void Start()
    {
        critRate *= .01f;
        critDamage *= .01f;

        float averageDamage = ((1 - critRate) * normalDamage) + (critRate * (1 + critDamage) * normalDamage);
        Debug.Log("Average Damage: " + averageDamage);
    }
}
