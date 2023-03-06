using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReseter : MonoBehaviour
{
    public void ResetData()
    {
        GameManager.instance.dataManager.ResetData();
    }
}
