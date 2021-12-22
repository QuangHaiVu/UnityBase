using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefsSerializeProvider", menuName = "SaveLoadSystem/PlayerPrefsSerializeProvider")]
public class PlayerPrefsSerializeProvider : SerializeProvider
{
    private string Path(string fileName)
    {
        return fileName;
    }

    public override string Read(string fileName)
    {
        try
        {
            //return ObscuredPrefs.GetString(this.Path(fileName));
            return PlayerPrefs.GetString(this.Path(fileName));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            //TODO:  Load Finish
        }
    }

    public override void Write(string data, string fileName, Action<bool> isWriteDone)
    {
        try
        {
            //ObscuredPrefs.SetString(fileName,data);
            PlayerPrefs.SetString(fileName, data);
        }
        catch (Exception ex)
        {
            isWriteDone(false);
            throw new Exception(ex.Message);
        }
        finally
        {
            //TODO: Save Finish
            isWriteDone(true);
        }
    }
}

