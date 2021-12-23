using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Utils
{
    #region PlayerPrefs

    public static bool IsExistPrefs(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void SetPlayerPrefsValue(string key, PlayerPrefsType type, int value)
    {
        if (!IsCorrectValue(type, value)) return;
        PlayerPrefs.SetInt(key, value);
    }

    public static void SetPlayerPrefsValue(string key, PlayerPrefsType type, float value)
    {
        if (!IsCorrectValue(type, value)) return;
        PlayerPrefs.SetFloat(key, value);
    }

    public static void SetPlayerPrefsValue(string key, PlayerPrefsType type, string value)
    {
        if (!IsCorrectValue(type, value)) return;
        PlayerPrefs.SetString(key, value);
    }

    public static T GetPlayerPrefsValue<T>(string key)
    {
        var type = typeof(T);
        object value = null;
        
        if (type == typeof(int))
        {
            value = Convert.ChangeType(PlayerPrefs.GetInt(key, 0), type);
        }

        if (type == typeof(string))
        {
            value = Convert.ChangeType(PlayerPrefs.GetString(key, ""), type);
        }

        if (type == typeof(float))
        {
            value = Convert.ChangeType(PlayerPrefs.GetFloat(key, 0f), type);
        }

        return (T) value;
    } 

    private static bool IsCorrectValue<T>(PlayerPrefsType type, T value)
    {
        bool isTrue = false;

        switch (type)
        {
            case PlayerPrefsType.Float:
                isTrue = value is float;
                break;
            case PlayerPrefsType.Int:
                isTrue = value is int;
                break;
            case PlayerPrefsType.String:
                isTrue = value is string;
                break;
        }

        if (!isTrue)
        {
            Debug.LogError($"{value} is not type of {type.ToString()}");
        }

        return isTrue;
    }

    #endregion

    #region Files
    
    public static bool ExistsFile(string name)
    {
        return File.Exists(FileNameToPath(name));
    }

    public static string FileNameToPath(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        return path;
    }

    #endregion

    #region Data

    public static void SaveData<T>(string fileName, object data)
    {
        try
        {
            if (data == null) return;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located

            var path = FileNameToPath(fileName + ".gd");
            if (File.Exists(path))
            {
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, (T)data);   
                    file.Close();
                    Debug.Log("[SaveData] Done: " + fileName + " " + DateTime.Now + "\n" + path);
                }
            }
            else
            {
                using (FileStream file = File.Create(path))
                {
                    binaryFormatter.Serialize(file, (T)data);
                    file.Close();
                    Debug.Log("[SaveData] Done: " + fileName + "\n" + path);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[SaveData] Exception: " + fileName + " " + ex.Message);
        }
        finally
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public static object LoadData<T>(string fileName)
    {
        try
        {
            var data = default(T);
            var path = FileNameToPath(fileName + ".gd");
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    data = (T)binaryFormatter.Deserialize(file);
                    file.Close();
                    Debug.Log("[LoadData] Done: " + fileName);
                }

                return data;
            }
            else
            {
                Debug.LogWarning("[LoadData] Error " + fileName + " " + "NOT found");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[LoadData]: Exception: " + fileName + " " + ex.Message);
            return null;
        }
        finally
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public static void DeleteData(string fileName)
    {
        try
        {
            var path = FileNameToPath(fileName + ".gd");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.LogWarning("[DeleteData] Error " + fileName + " " + "NOT found");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[DeleteData]: Exception: " + fileName + " " + ex.Message);
        }
        finally
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    #endregion

    #region Enums

    public static T ParseEnum<T>(string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

    #endregion

}

public enum PlayerPrefsType
{
    None = 0,
    Float = 1,
    Int = 2,
    String = 3,
}