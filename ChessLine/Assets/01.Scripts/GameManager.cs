using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerMaxHp;
    public int playerHp;
    public float spawnCooldown;
    public float moveCooldown;
    public float routineGap;
    public float deadRespawnTime;

    public int requiredExp;
    public int curExp;
    public int level = 1;

    private Coroutine coroutine;
    private bool isPlayerAlive = true;
    private bool isTimerEnd = false;
    public int difficulty;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름 검사
        if (scene.name == "MainScene")
        {
            Debug.Log("메인씬 로드됨!");
            StartRoutine();
        }
        else
        {
            Debug.Log("게임 종료!");
            StopCoroutine(coroutine);
            coroutine = null;

            StopAllGameRoutines();
        }
    }


    public void StartRoutine()
    {
        SaveLoadManager.Instance.Load();
        coroutine = StartCoroutine(StartState());
        difficulty = PlayerPrefs.GetInt("difficulty", 0);
        CreditManager.Instance.UpdateCreditUI();
    } 

    IEnumerator StartState()
    {
        // 플레이어 체력 초기화
        playerHp = playerMaxHp;
        isPlayerAlive = true;
        UpdateGameUI();
        TileSpawner.Instance.spawnNewMap();

        // 피스 상태 루틴 시작 - 이후 자동으로 돌아감.
        PieceManager.Instance.startPieceStateRoutine();

        coroutine = StartCoroutine(SpawnState());
        yield return null;
    }

    IEnumerator SpawnState()
    {
        yield return new WaitForSeconds(spawnCooldown);

        // 몬스터 랜덤 소환
        MonsterManager.Instance.spawnRandMonsterPool();

        coroutine = StartCoroutine(MoveState());
        yield return null;
    }

    IEnumerator MoveState()
    {
        yield return new WaitForSeconds(moveCooldown);

        // 몬스터 이동
        MonsterManager.Instance.MoveMonstersFoward();

        coroutine = StartCoroutine(IdleState());
        yield return null;
    }

    IEnumerator IdleState()
    {
        yield return new WaitForSeconds(routineGap);

        // 플레이어 상태, 몬스터 상태, 피스 상태 검사 후 분기

        // 플레이어 생존 검사
        if (!isPlayerAlive)
        {
            coroutine = StartCoroutine(EndState());
        }
        else
        {
            coroutine = StartCoroutine(SpawnState());
        }

        // 게임 자동 저장
        SaveLoadManager.Instance.Save();

        yield return null;
    }

    IEnumerator EndState()
    {

        // 플레이어 체력 소진으로 인한 게임 일시정지
        yield return new WaitForSeconds(routineGap);

        // 모든 몬스터 제거
        MonsterManager.Instance.DespawnAllMonster();

        // 모든 생성된 타일 제거
        TileSpawner.Instance.DespawnMap();

        // 모든 피스 정지상태
        PieceManager.Instance.StopPieceStateRoutine();

        // 사망 ui 활성화 > 이거 굳이 싶다?
        UIManager.Instance.OpenUI(UIManager.Instance.dieUI.gameObject);

        //  타이머 종료까지 대기
        bool flag = false;
        StartCoroutine(Timer(deadRespawnTime, Color.red));
        while (!isTimerEnd)
        {       
            yield return new WaitForFixedUpdate();           

            Debug.Log($"Waiting for Timer flag: {flag}");
        }
        isTimerEnd = false;

        // 대기 후 게임 재기동
        Debug.Log("Dead Timer over");
        coroutine = StartCoroutine(StartState());
        UIManager.Instance.CloseUI(UIManager.Instance.dieUI.gameObject);


        yield return null;
    }

    IEnumerator Timer(float waitTime, Color color)
    {
        // 동작 확인 완료
        float t = 0f;
        while(t < waitTime)
        {
            t += Time.fixedDeltaTime;
            UIManager.Instance.dieUI.UpdateTimerText(waitTime - t);

            yield return new WaitForFixedUpdate();
        }
        isTimerEnd = true;
        yield return true;
    }

    public void GetDamage(int value)
    {
        playerHp -= value;

        if(playerHp <= 0)
        {
            isPlayerAlive = false;
            playerHp = 0;
        }

        UIManager.Instance.gameUI.UpdateHPBar();
    }

    public void GetExp(int value)
    {
        curExp += value;

        if(curExp >= Mathf.Pow(level, 2) * 100)
        {
            level++;
            curExp -= (int)Mathf.Pow(level - 1, 2) * 100;
        }
        UIManager.Instance.gameUI.UpdateExpBar();
    }

    void UpdateGameUI()
    {
        UIManager.Instance.gameUI.UpdateHPBar();
        UIManager.Instance.gameUI.UpdateExpBar();
    }

    void StopAllGameRoutines()
    {
        PieceManager.Instance.StopPieceStateRoutine();
        MonsterManager.Instance.DespawnAllMonster();
        TileSpawner.Instance.DespawnMap();
    }
}
