using System;
using System.Collections;
using UnityEngine;

public class DataManager : PersistentSingleton<DataManager>
{
    public static Action OnDataLoaded = null;

    [SerializeField] private SerializeSettings settings;
    [SerializeField] private SerializeProvider provider;
    [SerializeField] private UserData userData;
    [SerializeField] private SettingData settingData;


    #region Save/Load

    private void Save<T>(BaseData data, Action<bool> action) where T : BaseData
    {
        this.provider.Write(this.settings.Convert(data), typeof(T).Name,
            b =>
            {
                if (b)
                {
                    action?.Invoke(true);
                }
            }
        );
    }

    public void Load<T>(BaseData baseData) where T : BaseData
    {
        this.settings.Serialize<BaseData>(this.provider.Read(typeof(T).Name), baseData);
    }

    #endregion

    #region UserData

    private void LoadUserData()
    {
        var elapsedTime = 0f;
        var data = this.provider.Read(nameof(UserData));

        StartCoroutine(DoLoadData());
        
        IEnumerator DoLoadData()
        {
            while (string.IsNullOrEmpty(data))
            {
                if (elapsedTime < 3f)
                {
                    Debug.LogWarning("UserData loading... " + elapsedTime.ToString("0.0"));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    break;
                }
            }
            
            if (string.IsNullOrEmpty(data))
            {
                //TODO Create new userdata here
                //Example
                //userdata.abc = xyz
            
                SaveUserData();
            }else
            {
                Load<UserData>(userData);
            }
            
            yield return new WaitForEndOfFrame();

            OnDataLoaded?.Invoke();
        }
    }

    public void SaveUserData()
    {
        Save<UserData>(userData, null);
    }
    
    #endregion

    #region Setting Data

    public void SaveSettingData()
    {
        Save<SettingData>(settingData, null);
    }

    private void LoadSettingData()
    {
        var data = this.provider.Read(nameof(SettingData));

        if (string.IsNullOrEmpty(data))
        {
            settingData.isMusic = true;
            settingData.isSound = true;
            settingData.isVibration = true;
            SaveSettingData();
        }
        else
        {
            Load<SettingData>(settingData);
        }
    }

    public SettingData GetSettingData()
    {
        return settingData;
    }

    #endregion

    private void Start()
    {
        LoadSettingData();
        LoadUserData();
    }
}