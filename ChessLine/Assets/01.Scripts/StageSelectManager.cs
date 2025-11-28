using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    public void SelectStage(int dif)
    {
        PlayerPrefs.SetInt("difficulty", dif);
        SceneManager.LoadScene("MainScene");
    }
}
