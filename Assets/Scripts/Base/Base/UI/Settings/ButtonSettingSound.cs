using System;
using UnityEngine;

public class ButtonSettingSound : ButtonSettingBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameSettingValue.OnSoundChanged += ListenEvent;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        GameSettingValue.OnSoundChanged -= ListenEvent;
    }
    
    protected override void Start()
    {
        base.Start();
        ChangeState(GameSettingValue.EnableSound);
    }

    protected override void SwitchOnOff()
    {
        GameSettingValue.EnableSound = !GameSettingValue.EnableSound;
        PlayerPrefs.SetInt(GameSettingValue.SOUND_SETTING, Convert.ToInt32(GameSettingValue.EnableSound));
        ChangeState(GameSettingValue.EnableSound);
    }
}