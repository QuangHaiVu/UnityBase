using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    
    [Header("Default Parameters")]
    /// the initial parameters
    [Tooltip("the initial parameters")]
    public UnitControllerParameters DefaultParameters;
    
    /// the current speed of the character
    public Vector2 Speed { get{ return _speed; } }
    
    
    // private local references		
    protected Vector2 _speed;
    protected Vector2 _externalForce;
    
    /// <summary>
    /// Use this to add force to the character
    /// </summary>
    /// <param name="force">Force to add to the character.</param>
    public virtual void AddForce(Vector2 force)
    {
        _speed += force;
        _externalForce += force;
        ClampSpeed();
        ClampExternalForce();
    }

    /// <summary>
    ///  use this to set the horizontal force applied to the character
    /// </summary>
    /// <param name="x">The x value of the velocity.</param>
    public virtual void AddHorizontalForce(float x)
    {
        _speed.x += x;
        _externalForce.x += x;
        ClampSpeed();
        ClampExternalForce();
    }

    /// <summary>
    ///  use this to set the vertical force applied to the character
    /// </summary>
    /// <param name="y">The y value of the velocity.</param>
    public virtual void AddVerticalForce(float y)
    {
        _speed.y += y;
        _externalForce.y += y;
        ClampSpeed();
        ClampExternalForce();
    }

    /// <summary>
    /// Use this to set the force applied to the character
    /// </summary>
    /// <param name="force">Force to apply to the character.</param>
    public virtual void SetForce(Vector2 force)
    {
        _speed = force;
        _externalForce = force;
        ClampSpeed();
        ClampExternalForce();
    }

    /// <summary>
    ///  use this to set the horizontal force applied to the character
    /// </summary>
    /// <param name="x">The x value of the velocity.</param>
    public virtual void SetHorizontalForce(float x)
    {
        _speed.x = x;
        _externalForce.x = x;
        ClampSpeed();
        ClampExternalForce();
    }

    /// <summary>
    ///  use this to set the vertical force applied to the character
    /// </summary>
    /// <param name="y">The y value of the velocity.</param>
    public virtual void SetVerticalForce(float y)
    {
        _speed.y = y;
        _externalForce.y = y;
        ClampSpeed();
        ClampExternalForce();
    }
    
    protected virtual void ClampSpeed()
    {
        _speed.x = Mathf.Clamp(_speed.x,-DefaultParameters.MaxVelocity.x,DefaultParameters.MaxVelocity.x);
        _speed.y = Mathf.Clamp(_speed.y,-DefaultParameters.MaxVelocity.y,DefaultParameters.MaxVelocity.y);
    }

    protected virtual void ClampExternalForce()
    {
        _externalForce.x = Mathf.Clamp(_externalForce.x,-DefaultParameters.MaxVelocity.x,DefaultParameters.MaxVelocity.x);
        _externalForce.y = Mathf.Clamp(_externalForce.y,-DefaultParameters.MaxVelocity.y,DefaultParameters.MaxVelocity.y);
    }
}