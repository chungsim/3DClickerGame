using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SaveData
{
    public int level;
    public int credits;
    public int exp;
    public int makedDiceNum;

    public List<SaveDiceData> ownedDiceList;
    public SaveDiceData[] equidDiceList = new SaveDiceData[8];
}

[System.Serializable]
public class SaveDiceData
{
    public int code;
    public PieceType[] pieceTypes;
}


public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private const string SAVE_KEY = "PLAYER_SAVE";

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Save()
    {
        SaveData data = new SaveData();
        data.level = GameManager.Instance.level;
        data.credits = CreditManager.Instance.credit;
        data.exp = GameManager.Instance.curExp;
        data.makedDiceNum = InvenManager.Instance.makedDiceNum;

        // DiceData → ID 저장
        data.ownedDiceList = new List<SaveDiceData>();

        for(int i = 0; i < InvenManager.Instance.dices.Count; i++)
        {
            data.ownedDiceList.Add(new SaveDiceData());
            data.ownedDiceList[i].code = InvenManager.Instance.dices[i].diceCode;
            data.ownedDiceList[i].pieceTypes = InvenManager.Instance.dices[i].diceFaces;
        }


        for(int i = 0; i < InvenManager.Instance.equipedDices.Length; i++)
        {
            if(InvenManager.Instance.equipedDices[i] == null)
            {
                continue;
            }
            else
            {
                
                SaveDiceData temp = new SaveDiceData();
                temp.code = InvenManager.Instance.equipedDices[i].diceCode;
                temp.pieceTypes = InvenManager.Instance.equipedDices[i].diceFaces;
                data.equidDiceList[i] = temp;
            }         
        }

        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save(); // ← 반영 필수
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        ApplySave(JsonUtility.FromJson<SaveData>(json));
    }

    public void ApplySave(SaveData data)
    {
        GameManager.Instance.level = data.level;
        CreditManager.Instance.credit = data.credits;
        GameManager.Instance.curExp = data.exp;
        InvenManager.Instance.makedDiceNum = data.makedDiceNum;

        InvenManager.Instance.dices = new List<DiceData>();
        for(int i = 0; i < data.ownedDiceList.Count; i++)
        {
            InvenManager.Instance.dices.Add(new DiceData());
            InvenManager.Instance.dices[i].diceCode = data.ownedDiceList[i].code;
            InvenManager.Instance.dices[i].diceFaces = data.ownedDiceList[i].pieceTypes;
        }

        for(int i = 0; i < data.equidDiceList.Length; i++)
        {
            if(data.equidDiceList[i].pieceTypes.Length < 1) 
            {
                continue;
            }
            else
            {
                DiceData temp = new DiceData();
                temp.diceCode = data.equidDiceList[i].code;
                temp.diceFaces = data.equidDiceList[i].pieceTypes;
                InvenManager.Instance.equipedDices[i] = temp;
                InvenManager.Instance.EquipDice(i, temp);
            }      
        }
    }
}
