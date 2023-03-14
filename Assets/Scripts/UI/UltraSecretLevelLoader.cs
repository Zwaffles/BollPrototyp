using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSecretLevelLoader : MonoBehaviour
{
    public void LoadUltraSecretLevel()
    {
        GameManager.instance.courseManager.LoadCourse(1, 0);
    }
}
