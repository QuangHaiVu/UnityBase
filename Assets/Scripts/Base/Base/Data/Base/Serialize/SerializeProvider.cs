using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SerializeProvider : ScriptableObject
{
    public abstract void Write(string data, string fileName, Action<bool> isWriteDone);
    public abstract string Read(string fileName);
}
