using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public static MonsterManager Instance { get; private set; }

    [SerializeField] private GameObject monsterFrame;
    private Vector3 monsterStartPos = new Vector3(0, 0.5f, 7);
    [SerializeField] private MonsterPool[] monsterPools;

    public List<GameObject[]> spawnedMonsters = new List<GameObject[]>();


    private Vector3 monsterYOffset = new Vector3(0f, 0.5f, 0f);
    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MoveMonstersFoward()
    {
        RaycastHit hit;
        Tile tile;
        for(int i = 0; i < spawnedMonsters.Count; i++)
        {
            for(int j = 0; j < spawnedMonsters[i].Length; j++)
            {
                if(spawnedMonsters[i][j] != null)
                {
                    Vector3 targetPos = spawnedMonsters[i][j].transform.position + Vector3.back;
                    Ray ray = new Ray(targetPos + Vector3.down * 5f, Vector3.up * 11f);
                    if(Physics.Raycast(ray, out hit))
                    {
                        if(hit.collider.TryGetComponent<Tile>(out tile))
                        {
                            if(!tile.isMonsterOn && !tile.isPieceOn)
                            {                              
                                spawnedMonsters[i][j].transform.position = tile.transform.position + monsterYOffset;
                                // 이전 타일 정보 삭제
                                spawnedMonsters[i][j].GetComponent<Monster>().tile.isMonsterOn = false;
                                spawnedMonsters[i][j].GetComponent<Monster>().tile.character = null;

                                // 새로 옮긴 타일의 정보 초기화
                                tile.isMonsterOn = true;
                                tile.character = spawnedMonsters[i][j].GetComponent<Monster>();
                                spawnedMonsters[i][j].GetComponent<Monster>().tile = tile;
                            }
                        }

                        if(MonsterReachEnd(spawnedMonsters[i][j]))
                        {
                            Debug.Log($"{spawnedMonsters[i][j].name} : Monster Reachs to End");
                            
                            tile.isMonsterOn = false;
                            tile.character = null;
                            GameManager.Instance.GetDamage(spawnedMonsters[i][j].GetComponent<Monster>().atk);
                            Destroy(spawnedMonsters[i][j]);
                        }
                    }
                }
            }
        }
    }

    public void spawnRandMonsterPool()
    {
        SpawnMonsters(GetRandMonsterPool());
    }

    void SpawnMonsters(MonsterPool monsterPool)
    {
        RaycastHit hit;
        Tile tile;
        GameObject[] tempArray = new GameObject[8];
        for(int i = 0; i < monsterPool.monsters.Length; i++)
        {
            if(monsterPool.monsters[i] == null) continue;
            GameObject go = Instantiate(monsterFrame, transform);

            Monster monster;
            if(go.TryGetComponent<Monster>(out monster))
            {
                monster.LoadMonsterData(monsterPool.monsters[i]);
                tempArray[i] = go;             
            }
            else
            {
                Destroy(go);
            }

            go.transform.position = new Vector3(i, 0, 0) + monsterStartPos;

            Ray ray = new Ray(go.transform.position + Vector3.down * 5f, Vector3.up * 11f);
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.TryGetComponent<Tile>(out tile))
                    {
                        Debug.Log("tile hit");
                        go.GetComponent<Monster>().tile = tile;
                        tile.isMonsterOn = true;
                        tile.character = monster;
                    }
                }
        }
        spawnedMonsters.Add(tempArray);
    }
    MonsterPool GetRandMonsterPool()
    {
        return monsterPools[Random.Range(0, monsterPools.Length)]; 
    }

    bool MonsterReachEnd(GameObject movedMonster)
    {
        if(movedMonster.transform.position.z < 1)
        {
            return true;
        }

        return false;
    }

    public void DespawnAllMonster()
    {
        foreach(GameObject[] monsters in spawnedMonsters)
        {
            for(int i = monsters.Length -1; i >= 0; i--)
            {
                Destroy(monsters[i]);
            }
        }

        spawnedMonsters.Clear();

        Debug.Log("Despawn All Monsters");
    }

    public void DespawnMonster(Monster monster)
    {
        for(int i = spawnedMonsters.Count -1; i >= 0; i--)
        {
            for(int j = spawnedMonsters[i].Length -1; j >= 0; j--)
            {
                if(spawnedMonsters[i][j] == monster.gameObject)
                {
                    spawnedMonsters[i][j] = null;
                    Destroy(monster.gameObject);
                }
            }
        }
    }
}
