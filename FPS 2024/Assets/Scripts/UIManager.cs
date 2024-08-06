using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerUi;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        timerUi = GameObject.Find("TimerUiMaster").GetComponent<TextMeshProUGUI>();
    }
    public void SetSeeTimerOver(bool isMaster)
    {
        timerUi.enabled = isMaster;
    }
    public void UpdateTextTimer(float timer)
    {
        timerUi.text = "Timer: " + (int)timer;
    }
}
