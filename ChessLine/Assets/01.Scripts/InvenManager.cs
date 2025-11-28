using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvenManager : MonoBehaviour
{
    public static InvenManager Instance;

    public List<DiceData> dices;
    public DiceData[] equipedDices = new DiceData[8];

    public int makedDiceNum = 1;
    public int makedDicePrice = 100;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetDice(DiceData diceData)
    {
        dices.Add(diceData);
    }

    public void PopDice(DiceData diceData)
    {
        for(int i = dices.Count -1 ; i >= 0; i--)
        {
            if(diceData.diceCode == dices[i].diceCode)
            {
                dices.Remove(diceData);
                break;
            }
        }
    }

    public void EquipDice(int index, DiceData diceData)
    {
        // 다이스 장착 정보 등록
        equipedDices[index] = diceData;
        // 해당 피스에 다이스 등록
        PieceManager.Instance.pieceStateMachines[index].diceData = diceData; 
    }

    public void UnequipDice(int index)
    {
        // 장착할려는 위치에 있는 주사위 인벤에 되돌리기
        if(equipedDices[index] != null)
        {
            GetDice(equipedDices[index]);

            // 다이스 장착 정보 삭제
            equipedDices[index] = null;
            // 해당 피스의 다이스 정보 삭제 
            PieceManager.Instance.pieceStateMachines[index].diceData = null;
        }
    }

    public void DeleteDice(DiceData diceData)
    {
        for(int i = dices.Count -1 ; i >= 0; i--)
        {
            if(diceData.diceCode == dices[i].diceCode)
            {
                dices.Remove(diceData);
                break;
            }
        }

        CreditManager.Instance.GetCredit(makedDicePrice / 2);
    }

    public DiceData MakeRandomDice()
    {
        DiceData temp = new DiceData();
        temp.diceFaces= new PieceType[6];
        PieceType[] types = (PieceType[])System.Enum.GetValues(typeof(PieceType));

        for(int i = 0; i < temp.diceFaces.Length; i++)
        {
            int ranInt = Random.Range(0, 6);
            temp.diceFaces[i] = types[ranInt];
        }

        temp.diceCode = makedDiceNum;
        makedDiceNum++;

        return temp;
    }
}
