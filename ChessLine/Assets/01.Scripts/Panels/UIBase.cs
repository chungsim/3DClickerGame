using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    public void FillUiImage(Image image, float value)
    {
        image.fillAmount = value;
    }

    public void UpdateText(TextMeshProUGUI textUGUI, string value)
    {
        textUGUI.text = value;
    }
}
