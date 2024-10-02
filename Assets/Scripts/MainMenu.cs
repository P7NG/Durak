using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Image _soundButton;
    [SerializeField] private GameObject _settingsPanel;

    private bool _hasSound = true;
    public void ChangeSound()
    {
        if (_hasSound)
        {
            _hasSound = false;
            _soundButton.sprite = _offSprite;
        }
        else
        {
            _hasSound = true;
            _soundButton.sprite = _onSprite;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenAndCloseSettingsPanel()
    {
        if (_settingsPanel.activeInHierarchy)
        {
            _settingsPanel.SetActive(false);
        }
        else
        {
            _settingsPanel.SetActive(true);
        }
    }
}
