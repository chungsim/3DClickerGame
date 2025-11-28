using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIBase
{
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expText;

    [SerializeField] private TextMeshProUGUI creditText;
    [SerializeField] private TextMeshProUGUI levelText;
    public void UpdateHPBar()
    {
        FillUiImage(hpBar, (float)GameManager.Instance.playerHp / (float)GameManager.Instance.playerMaxHp);
        UpdateText(hpText, $"HP {GameManager.Instance.playerHp}/{GameManager.Instance.playerMaxHp}");
    }

    public void UpdateExpBar()
    {
        FillUiImage(expBar, (float)GameManager.Instance.curExp / ((float)Mathf.Pow(GameManager.Instance.level, 2) * 100f));
        UpdateText(expText, $"Exp {GameManager.Instance.curExp}/{(int)Mathf.Pow(GameManager.Instance.level, 2) * 100}");
        UpdateText(levelText, GameManager.Instance.level.ToString());
    }

    public void UpdateCredit()
    {
        int temp = CreditManager.Instance.credit;
        string outString = "";
        if(temp > 10000)
        {
            temp = temp / 1000;
            outString += temp.ToString() + "K";
        }
        else
        {
            outString = temp.ToString();
        }
        
        UpdateText(creditText, outString);
    }

}
