using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieUI : UIBase
{
    [SerializeField] private TextMeshProUGUI timerText;
    public void UpdateTimerText(float value)
    {
        UpdateText(timerText, value.ToString("n2"));
    }
}
