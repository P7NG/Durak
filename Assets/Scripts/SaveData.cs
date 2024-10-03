using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveData : MonoBehaviour
{
    public void Save(int GameTime, int StarCount)
    {
        YandexGame.savesData.GameTime = GameTime;
        YandexGame.savesData.StarCount = StarCount;        
        YandexGame.SaveProgress();
    }
}
