using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public static CreditManager Instance {get; private set; }
    public int credit;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetCredit(int value)
    {
        credit += value;
        UpdateCreditUI();
    }

    public void LoseCredit(int value)
    {
        if(credit >= value)
        {
            credit -= value;
        }
        else
        {
            Debug.Log("Not Enough Credits");
        }
        UpdateCreditUI();
    }

    public void UpdateCreditUI()
    {
        UIManager.Instance.gameUI.UpdateCredit();
    }
}
