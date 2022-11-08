using System;
using UnityEngine;

public class ButtonSettingMusic : ButtonSettingBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameSettingValue.OnMusicChanged += ListenEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameSettingValue.OnMusicChanged -= ListenEvent;
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(GameSettingValue.EnableMusic);
    }

    protected override void SwitchOnOff()
    {
        GameSettingValue.EnableMusic = !GameSettingValue.EnableMusic;
        PlayerPrefs.SetInt(GameSettingValue.MUSIC_SETTING, Convert.ToInt32(GameSettingValue.EnableMusic));
        ChangeState(GameSettingValue.EnableMusic);
    }
}