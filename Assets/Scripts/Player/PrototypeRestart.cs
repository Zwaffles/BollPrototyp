using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PrototypeRestart : MonoBehaviour
{
    private Vector3 restartPosition;

    private void Start()
    {

        restartPosition = transform.position;

    }

    void Update()
    {

        if (!Input.GetKeyDown(KeyCode.R)) return;

        transform.position = restartPosition;
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);

    }
}
