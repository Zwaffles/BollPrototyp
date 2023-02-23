using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Johan Rosenberg
 * 2023-02-21
 * Last modified: 2023-02-23
 */
public class TestBehaviourToggle : MonoBehaviour
{

    [SerializeField, Tooltip("The scripts for the behaviours that will be toggled on and off")]
    private MonoBehaviour behaviour1, behaviour2;

    [SerializeField, Tooltip("The colors which will represent each behaviour")]
    private Color color1, color2;

    private Material material;

    private int behaviourIterator = 0b00;

    // Start is called before the first frame update
    void Start()
    {

        behaviour1.enabled = false;
        behaviour2.enabled = false;

        material = GetComponentInChildren<MeshRenderer>().material;

    }

    // Update is called once per frame
    void Update()
    {

        bool shouldSwitchBehaviours = Input.GetKeyDown(KeyCode.B);

        if (!shouldSwitchBehaviours) return;

        /*
         *  Sorry, I got bored and *had* to write bad code.
         *  Dw, this is meant only for the prototyping purposes
         */

        behaviourIterator = (behaviourIterator + 1) & 0b11;

        switch (behaviourIterator)
        {

            case 0b00:
                behaviour1.enabled = false;
                behaviour2.enabled = false;
                material.color = Color.white;
                break;
            case 0b01:
                behaviour1.enabled = true;
                material.color = color1;
                break;
            case 0b10:
                behaviour1.enabled = false;
                behaviour2.enabled = true;
                material.color = color2;
                break;
            case 0b11:
                behaviour1.enabled = true;
                material.color = Color.Lerp(color1, color2, 0.5f);
                break;

        }

        shouldSwitchBehaviours = false;

    }
}
