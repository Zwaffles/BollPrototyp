using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject debugPanel1;
    [SerializeField]
    private GameObject debugPanel2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            debugPanel1.SetActive(!debugPanel1.activeInHierarchy);
            debugPanel2.SetActive(!debugPanel2.activeInHierarchy);
        }
    }
}
