using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualComponent : MonoBehaviour
{
    public Text NickName;
    public string[] Names;
    public Image MessageBG;
    public Text MessageText;
    public Text ButtonText;
    public GameObject WinPanel;
    public GameObject LosePanel;

    void Start()
    {
        SetName();
    }

    private void SetName()
    {
        NickName.text = Names[Random.Range(0, Names.Length - 1)];
    }

    public void Win()
    {
        WinPanel.SetActive(true);
    }

    public void Lose()
    {
        LosePanel.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitWithStar()
    {
        //вызов рекламы
    }

    public void WriteMessage(string messageText)
    {
        StartCoroutine(CloseMessageWindow(messageText));
    }

    public void ChangeButtonStatus(string text)
    {
        ButtonText.text = text;
    }

    private IEnumerator CloseMessageWindow(string messageText)
    {
        MessageBG.enabled = true;
        MessageText.text = messageText;
        yield return new WaitForSeconds(2);
        MessageText.text = string.Empty;
        MessageBG.enabled = false;
    }
}
