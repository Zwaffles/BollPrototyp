using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UseBoost: MonoBehaviour
{
    private VisualElement _Boost1;
    private VisualElement _Boost2;
    private VisualElement _Boost3;


    private void Container_TransitionEnd(TransitionEndEvent evt)
    {
        _Boost1.style.backgroundColor = new StylebackgroundColor(new backgroundColor(0, 0, 0));
        _Boost1.style.colors.background = new 

        if (destroyOnTransitionEnd)
            Destroy(gameObject, duration);
    }

    private void Container_TransitionEnd(TransitionEndEvent evt)
    {
        _Boost2.style.translate = new StyleTranslate(new Translate(0, 0, 0));

        if (destroyOnTransitionEnd)
            Destroy(gameObject, duration);
    }

    private void Container_TransitionEnd(TransitionEndEvent evt)
    {
        _Boost3.style.translate = new StyleTranslate(new Translate(0, 0, 0));

        if (destroyOnTransitionEnd)
            Destroy(gameObject, duration);
    }
}