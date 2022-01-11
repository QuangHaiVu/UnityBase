using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AutoRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [Tooltip("if this is true, this object will be repositioned at its initial position when the player revives")]
    public bool RepositionOnRespawn = false;

    [Tooltip("whether or not this auto respawn should disable its game object when Kill is called")]
    public bool DisableRendererOnKill = true;

    [Header("Auto respawn after X seconds")]
    [Tooltip(
        "if this has a value superior to 0, this object will respawn at its last position X seconds after its death")]
    public float AutoRespawnDuration = 0f;

    [Tooltip("the amount of times this object can auto respawn, negative value : infinite")]
    public int AutoRespawnAmount = 3;

    [Tooltip("the remaining amounts of respawns (readonly, controlled by the class at runtime)")] [MMReadOnly]
    public int AutoRespawnRemainingAmount = 3;

    [Header("Feedbacks")] [Tooltip("the MMFeedbacks to play when the player respawns")]
    public MMFeedbacks RespawnFeedback;

    protected MonoBehaviour[] _otherComponents;
    protected Collider2D _collider2D;
    protected Renderer _renderer;
    protected Health _health;
    protected bool _reviving = false;
    protected float _timeOfDeath = 0f;
    protected Vector3 _initialPosition;
    public UnityEvent OnRespawn;

    /// <summary>
    /// On Start we grab our various components
    /// </summary>
    protected virtual void Start()
    {
        AutoRespawnRemainingAmount = AutoRespawnAmount;
        _otherComponents = this.gameObject.GetComponents<MonoBehaviour>();
        _collider2D = this.gameObject.GetComponent<Collider2D>();
        _renderer = this.gameObject.GetComponent<Renderer>();
        _health = this.gameObject.GetComponent<Health>();
        _initialPosition = this.transform.position;
    }

    /// <summary>
    /// On Update we check whether we should be reviving this agent
    /// </summary>
    protected virtual void Update()
    {
        if (_reviving)
        {
            if (_timeOfDeath + AutoRespawnDuration <= Time.time)
            {
                if (AutoRespawnAmount == 0)
                {
                    return;
                }

                if (AutoRespawnAmount > 0)
                {
                    if (AutoRespawnRemainingAmount <= 0)
                    {
                        return;
                    }

                    AutoRespawnRemainingAmount -= 1;
                }

                Revive();
                _reviving = false;
            }
        }
    }

    /// <summary>
    /// Kills this object, turning its parts off based on the settings set in the inspector
    /// </summary>
    public virtual void Kill()
    {
        if (AutoRespawnRemainingAmount <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (DisableRendererOnKill)
        {
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }
        }

        foreach (MonoBehaviour component in _otherComponents)
        {
            if (component != this)
            {
                component.enabled = false;
            }
        }

        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }

        _reviving = true;
        _timeOfDeath = Time.time;
    }

    /// <summary>
    /// Revives this object, turning its parts back on again
    /// </summary>
    public virtual void Revive()
    {
        if (_health != null)
        {
            _health.Revive();
        }


        foreach (MonoBehaviour component in _otherComponents)
        {
            component.enabled = true;
        }

        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }

        if (_renderer != null)
        {
            _renderer.enabled = true;
        }

        if (RepositionOnRespawn)
        {
            this.transform.position = _initialPosition;
        }

        RespawnFeedback?.PlayFeedbacks();

        if (OnRespawn != null)
        {
            OnRespawn.Invoke();
        }
    }
}