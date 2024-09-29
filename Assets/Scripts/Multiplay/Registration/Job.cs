using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Job : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    public List<string> CurrentJobList;
    private Dictionary<string, long> jobDic;
    private int maxBitSize = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentJobList = new List<string>();
        jobDic = new Dictionary<string, long>();

        int dicIndex = 1;

        foreach (var job in dropdown.options)
        {
            jobDic.Add(job.text, dicIndex++);
        }

        maxBitSize = (int)Mathf.Pow((float)2, (float)dropdown.options.Count);
    }

    public void OnSelectDropdown()
    {
        CurrentJobList.Clear();

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            int temp = dropdown.value & (1 << i);
            if (temp == (1 << i))
            {
                CurrentJobList.Add(dropdown.options[i].text);
            }
        }
        Debug.Log(CurrentJobList);
    }

    public bool IsValidJob()
    {
        if (CurrentJobList.Count == 0)
            return false;
        else
            return true;
    }
}
