using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public static class GameSettingValue
{
    private static bool musicOn = true;
    private static bool soundOn = true;
    
    public static string MUSIC_SETTING = "Music";
    public static string SOUND_SETTING = "Sound";
    
    public static Action<bool> OnMusicChanged;
    public static Action<bool> OnSoundChanged;

    public static bool EnableMusic
    {
        get => musicOn;
        set
        {
            PlayerPrefs.SetInt(GameSettingValue.MUSIC_SETTING, Convert.ToInt32(value));
            musicOn = value;
            if (value)
            {
                MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Music);
            }
            else
            {
                MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Music);
            }
            OnMusicChanged?.Invoke(value);
        }
    }
    
    public static bool EnableSound
    {
        get => soundOn;
        set
        {
            PlayerPrefs.SetInt(GameSettingValue.SOUND_SETTING, Convert.ToInt32(value));
            soundOn = value;
            if (value)
            {
                MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
            }
            else
            {
                MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
            }
            OnSoundChanged?.Invoke(value);
        }
    }
}
