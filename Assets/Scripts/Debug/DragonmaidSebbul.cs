using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DragonmaidSebbul : MonoBehaviour
{

    VisualEffect[] visualEffects;

    void OnEnable()
    {

        visualEffects = gameObject.GetComponentsInChildren<VisualEffect>();

        foreach (VisualEffect visualEffect in visualEffects)

        if (visualEffect.HasVector4("ParticleColor"))
        {
            visualEffect.SetVector4("ParticleColor", Color.red);
        }

    }

}
