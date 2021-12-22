using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonSerializeSettings", menuName = "SaveLoadSystem/JsonSerializeSettings")]
public class JsonSerializeSettings : SerializeSettings
{
    public override string Convert(BaseData data)
    {
        return data == null ? string.Empty : JsonUtility.ToJson(data, this.isView);
    }

    public override void Serialize<T>(string data, BaseData serializedObject)
    {
        if (data == null)
        {
            return;
        }
        JsonUtility.FromJsonOverwrite(data, serializedObject);
    }

}
