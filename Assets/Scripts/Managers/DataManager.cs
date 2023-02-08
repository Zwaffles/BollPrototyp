using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private List<SetData> setDataList;

    private void Start()
    {
        LoadData();
    }

    public void SaveData(List<SetData> setDataList)
    {
        this.setDataList = setDataList;

        for (int i = 0; i < setDataList.Count; i++)
        {
            string jsonData = JsonUtility.ToJson(setDataList[i]);
            PlayerPrefs.SetString("SetData_" + i, jsonData);
        }

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        setDataList = new List<SetData>();
        int setDataCount = 0;

        while(PlayerPrefs.HasKey("SetData_" + setDataCount))
        {
            Debug.Log("Save data found");
            string jsonData = PlayerPrefs.GetString("SetData_" + setDataCount);

            if (!string.IsNullOrEmpty(jsonData))
            {
                SetData setData = JsonUtility.FromJson<SetData>(jsonData);
                setDataList.Add(setData);
            }

            setDataCount++;
        }

        GameManager.instance.courseManager.LoadPlayerProgress(setDataList);

        //for (int i = 0; i < setDataList.Count; i++)
        //{
        //    string jsonData = PlayerPrefs.GetString("SetData_" + i, "");

        //    if (!string.IsNullOrEmpty(jsonData))
        //    {
        //        SetData setData = JsonUtility.FromJson<SetData>(jsonData);
        //        setDataList.Add(setData);
        //        GameManager.instance.courseManager.LoadPlayerProgress(setDataList);
        //    }
        //}
    }
}
