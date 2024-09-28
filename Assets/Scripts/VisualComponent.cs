using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualComponent : MonoBehaviour
{
    public Text NickName;
    public string[] Names;
    public Image MessageBG;
    public Text MessageText;

    void Start()
    {
        SetName();
    }

    private void SetName()
    {
        NickName.text = Names[Random.Range(0, Names.Length - 1)];
    }

    public void WriteMessage(string messageText)
    {
        StartCoroutine(CloseMessageWindow(messageText));
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
