using UnityEngine;
public abstract class SerializeSettings : ScriptableObject
{
    [SerializeField] protected bool isView = false;
    public abstract string Convert(BaseData data);
    public abstract void Serialize<T>(string data, BaseData serializedObject) where T : BaseData;
}