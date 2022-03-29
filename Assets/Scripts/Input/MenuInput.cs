using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuInput : MonoBehaviour
{
    public List<Button> buttons;

    private void Start()
    {
        foreach (Button button in buttons)
        {
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }
    }

    private void TaskOnClick()
    {
        AudioManager.Instance.MenuEnterAudio(gameObject);
    }
}