using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControllerParameters
{
    [Header("Speed")]

    [Tooltip("Maximum velocity for your character, to prevent it from moving too fast on a slope for example")]
    public Vector2 MaxVelocity = new Vector2(100f, 100f);
}
