using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "FileSerializeProvider.Asset", menuName = "SaveLoadSystem/FileSerializeProvider")]
public class FileSerializeProvider : SerializeProvider
{
    private string Path(string fileName)
    {
        return $"{Application.persistentDataPath}/{fileName}";
    }

    /// <inheritdoc/>
    public override string Read(string fileName)
    {
        try
        {
            return File.ReadAllText(this.Path(fileName));
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

    /// <inheritdoc/>
    public override void Write(string data, string fileName, Action<bool> isWriteDone)
    {
        try
        {
            File.WriteAllText(this.Path(fileName), data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            //TODO: Save Finish
            isWriteDone(true);
        }
    }
}