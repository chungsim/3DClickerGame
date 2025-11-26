using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    private MonsterData initMonsterData;

    public Tile tile;

    private int dropGold;

    [SerializeField] private Image hpBar;

    public void LoadMonsterData(MonsterData monsterData)
    {
        if(monsterData != null)
        {
            maxHP = monsterData.maxHP;
            curHp = monsterData.maxHP;
            atk = monsterData.atk;
            def = monsterData.def;
            dropGold = monsterData.dropGold;

            initMonsterData = monsterData;
        }
    }

    public override void LoseHp(int value)
    {
        int cul = curHp - value + def;
        curHp = cul > 0 ? cul : 0;

        hpBar.fillAmount = (float)curHp / (float)maxHP;

        if(curHp < 1)
        {
            CreditManager.Instance.GetCredit(dropGold);
            GameManager.Instance.GetExp(dropGold);

            Die();
        }
    }

    private void Die()
    {
        tile.isMonsterOn = false;
        tile.character = null;

        MonsterManager.Instance.DespawnMonster(this);
    }
}
