using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxhHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Please add a 'Hat' component to your ball to prevent freezing");
        GameObject.Destroy(this.gameObject);
    }

}
