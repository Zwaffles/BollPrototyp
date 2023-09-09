using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UIColorManager : MonoBehaviour
{

    VisualEffect[] visualEffects;
    SpriteRenderer[] renderers;

    void OnEnable()
    {

        visualEffects = gameObject.GetComponentsInChildren<VisualEffect>();

        renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        GameManager.instance.uiManager.ProvideUIColorManager(this);

    }

    public void changeColors(Color primaryColor, Color secondaryColor)
    {

        foreach (VisualEffect visualEffect in visualEffects)
        {
            if (visualEffect.HasVector4("ParticleColor"))
            {
                visualEffect.SetVector4("ParticleColor", secondaryColor);
            }
        }

        foreach (SpriteRenderer renderer in renderers)
        {

            renderer.color = primaryColor;

        }

    }

}
