using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    public static InvenUI Instance; 
    public Transform contentParent; // Scroll View 안의 Content
    public GameObject DiceItemPrefab;

    private List<DiceData> invenDiceDatas = new List<DiceData>();
    private List<DiceItem> diceItems = new List<DiceItem>();

    public GameObject[] equipbuttonImages;

    private DiceItem selectedDiceItem;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        selectedDiceItem = null;

        PopulateUI();
    }

    public void PopulateUI()
    {
        // 인벤에 있는 다이스 정보 불러오기
        if(InvenManager.Instance.dices == null) return;

        invenDiceDatas = InvenManager.Instance.dices;

        // 이미 있는 주사위 아이템들 삭제
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        diceItems.Clear();

        // 주사위 수만큼 viewport의 content 밑에 자식으로 생성, 데이터 적용
        foreach (var diceData in invenDiceDatas)
        {
            GameObject obj = Instantiate(DiceItemPrefab, contentParent);
            DiceItem item = obj.GetComponent<DiceItem>();
            item.SetData(diceData);
            diceItems.Add(item);
        }

        for(int i = 0; i < equipbuttonImages.Length; i++)
        {
            if(InvenManager.Instance.equipedDices[i] != null)
            {
                equipbuttonImages[i].SetActive(true);
            }
            else
            {
                equipbuttonImages[i].SetActive(false);
            }
        }
    }

    public void SelectItem(DiceItem diceItem)
    {
        for(int i = 0; i < diceItems.Count; i++)
        {
            diceItems[i].isSelected = false;
        }

        diceItem.isSelected = true;

        selectedDiceItem = diceItem;
    }

    public void EquipDiceItem(int index)
    {
        if(selectedDiceItem == null) return;

        if(InvenManager.Instance.equipedDices[index] != null)
        {
            UnepuipDiceitem(index);
        }

        InvenManager.Instance.equipedDices[index] = selectedDiceItem.initDiceData;

        InvenManager.Instance.EquipDice(index, selectedDiceItem.initDiceData);
        InvenManager.Instance.PopDice(selectedDiceItem.initDiceData);

        selectedDiceItem = null;

        PopulateUI();
    }

    public void UnepuipDiceitem(int index)
    {
        if(InvenManager.Instance.equipedDices[index] == null) return;

        InvenManager.Instance.UnequipDice(index);
        InvenManager.Instance.equipedDices[index] = null;

        equipbuttonImages[index].SetActive(false);

        PopulateUI();
    }

    public void EquipButton(int index)
    {
        UnepuipDiceitem(index);

        if(selectedDiceItem != null)
        {
            EquipDiceItem(index);
            equipbuttonImages[index].SetActive(true);
        }
    }
}
