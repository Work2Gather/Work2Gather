using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    #region Serialized Member
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int timerSec;

    #endregion

    #region Private Member
    private int min;
    private int sec;
    private UnityAction onTimeOutEvent;

    #endregion

    public void StartTimer(int _timerSec)
    {
        timerSec = _timerSec;
        StartCoroutine(DoStartTimer());
    }

    public Timer OnTimeOut(UnityAction unityAction)
    {
        if (unityAction == null)
            return this;

        onTimeOutEvent = null;
        onTimeOutEvent = unityAction;

        return this;
    }

    private string PadZeroNumber(int _num)
    {
        return _num.ToString().PadLeft(2, '0');
    }

    private IEnumerator DoStartTimer()
    {
        string tempText = "0:00";

        while (timerSec >= 0)
        {
            double temp = timerSec / 60;
            min = (int)Math.Floor(temp);
            sec = timerSec % 60;

            tempText = $@"{min}:{PadZeroNumber(sec)}";
            timerText.text = tempText;

            yield return new WaitForSeconds(1);
            timerSec--;
        }
        timerSec = 0;
        Debug.Log("Timer Stopped");
        if (onTimeOutEvent != null)
            onTimeOutEvent.Invoke();
    }
}
