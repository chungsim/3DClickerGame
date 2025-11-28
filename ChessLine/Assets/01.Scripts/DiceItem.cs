using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceItem : MonoBehaviour
{
    [Header("UI References")]
    public Image[] faceIcons;
    public Image icon;
    public bool isSelected = false;

    public DiceData initDiceData;
    
    public void SetData(DiceData diceData)
    {
        for(int i = 0; i < faceIcons.Length; i++)
        {
            faceIcons[i].sprite = Datas.piceeIconFair[diceData.diceFaces[i]];
        }
        initDiceData = diceData;
        // 색상 변경 예시 (해금 시 밝게)
        //GetComponent<Image>().color = data.isUnlocked ? new Color(0.8f, 1f, 0.8f) : Color.white;
    }

    void Update()
    {
        if (isSelected)
        {
            for(int i = 0; i < faceIcons.Length; i++)
            {
                faceIcons[i].color = Color.blue;
            }

            icon.color = Color.blue;
        }
        else
        {
            for(int i = 0; i < faceIcons.Length; i++)
            {
                faceIcons[i].color = Color.black;
            }

            icon.color = Color.black;
        }
    }

    public void selectButton()
    {
        InvenUI.Instance.SelectItem(this);
    }
}
