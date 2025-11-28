using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Stack<GameObject> uiStack;
    [SerializeField] private GameObject[] uis;

    public GameUI gameUI;
    public DieUI dieUI;


    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenUI(GameObject targetUI)
    {
        targetUI.SetActive(true);
    }

    public void CloseUI(GameObject targetUI)
    {
        targetUI.SetActive(false);
    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
