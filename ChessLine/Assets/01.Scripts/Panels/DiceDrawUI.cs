using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceDrawUI : UIBase
{
    [SerializeField] private TextMeshProUGUI[] faceNums;

    [SerializeField] private GameObject okMessage;
    [SerializeField] private GameObject notOkMessage;
    public void SetFaceNum(DiceData diceData)
    {
        int[] tempNums = new int[]{0,0,0,0,0,0};

        foreach(PieceType pieceType in diceData.diceFaces)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    tempNums[0]++;
                    break;

                case PieceType.Rook:
                    tempNums[1]++;
                    break;

                case PieceType.Knight:
                    tempNums[2]++;
                    break;

                case PieceType.Bishop:
                    tempNums[3]++;
                    break;

                case PieceType.Queen:
                    tempNums[4]++;
                    break;

                case PieceType.King:
                    tempNums[5]++;
                    break;
            }

            for(int i = 0; i < faceNums.Length; i++)
            {
                faceNums[i].text = "X " + tempNums[i].ToString();
            }
        }
    }

    public void ClickDrawButton()
    {
        if(CreditManager.Instance.credit >= InvenManager.Instance.makedDicePrice)
        {
            CreditManager.Instance.LoseCredit(InvenManager.Instance.makedDicePrice);

            DiceData diceData = InvenManager.Instance.MakeRandomDice();
            InvenManager.Instance.GetDice(diceData);
            SetFaceNum(diceData);
            okMessage.SetActive(true);

        }
        else
        {
            Debug.Log("Not Enough Credits");
            notOkMessage.SetActive(true);
        }
    }
}
