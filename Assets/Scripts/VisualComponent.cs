using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class VisualComponent : MonoBehaviour
{
    public Text NickName;
    public string[] Names;
    public Image MessageBG;
    public Text MessageText;
    public Text ButtonText;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public YandexGame yg;

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
        if (WinPanel.activeInHierarchy)
        {
            YandexGame.savesData.StarCount += 2;
        }
        else
        {
            YandexGame.savesData.StarCount += 1;
        }

        SceneManager.LoadScene(0);
    }

    public void ExitWithStar()
    {
        YandexGame.RewVideoShow(1);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    void Rewarded(int id)
    {
        if (id == 1)
            AddStar();
    }

    public void AddStar()
    {
        if (WinPanel.activeInHierarchy) 
        {
            YandexGame.savesData.StarCount += 2;
        }
        else
        {
            YandexGame.savesData.StarCount += 1;
        }
        YandexGame.SaveProgress();
        Exit();
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
