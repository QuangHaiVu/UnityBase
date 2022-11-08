using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingLoader : MonoBehaviour
{
    private void Start()
    {
        var musicSetting = PlayerPrefs.GetInt(GameSettingValue.MUSIC_SETTING, 1);
        GameSettingValue.EnableMusic = musicSetting == 1 ? true : false;
        
        var soundSetting = PlayerPrefs.GetInt(GameSettingValue.SOUND_SETTING, 1);
        GameSettingValue.EnableSound = soundSetting == 1 ? true : false;
    }
}
