using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    /// the current health of the character
    [MMReadOnly] [Tooltip("the current health of the character")]
    public float CurrentHealth;

    [Header("Health")]
    [MMInformation(
        "Add this component to an object and it'll have health, will be able to get damaged and potentially die.",
        MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
    [Tooltip("the initial amount of health of the object")]
    public float InitialHealth = 10;

    [Tooltip("the maximum amount of health of the object")]
    public float MaximumHealth = 10;

    [Tooltip("if this is true, this object can't take damage")]
    public bool Invulnerable = false;

    [Header("Damage")]
    [MMInformation(
        "Here you can specify an effect and a sound FX to instantiate when the object gets damaged, and also how long the object should flicker when hit (only works for sprites).",
        MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
    [Tooltip("the MMFeedbacks to play when the character gets hit")]
    public MMFeedbacks DamageFeedbacks;

    [Header("Death")]
    [MMInformation(
        "Here you can set an effect to instantiate when the object dies, a force to apply to it (corgi controller required), how many points to add to the game score, and where the character should respawn (for non-player characters only).",
        MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
    [Tooltip("the MMFeedbacks to play when the character dies")]
    public MMFeedbacks DeathFeedbacks;

    [Tooltip("if this is not true, the object will remain there after its death")]
    public bool DestroyOnDeath = true;

    [Tooltip("the time (in seconds) before the character is destroyed or disabled")]
    public float DelayBeforeDestruction = 0f;

    [Tooltip("if this is true, collisions will be turned off when the character dies")]
    public bool CollisionsOffOnDeath = true;

    [Header("Actions")] public UnityEvent<float> OnHit;

    public UnityEvent OnHitZero;

    public UnityEvent OnDeath;

    public UnityEvent OnRevive;


    protected bool _initialized = false;
    protected MMHealthBar _healthBar;
    protected AutoRespawn _autoRespawn;
    protected Collider2D _collider2D;
    [SerializeField] protected AnimationController _animationController;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _collider2D = this.gameObject.GetComponent<Collider2D>();
        _autoRespawn = this.gameObject.GetComponent<AutoRespawn>();
        _healthBar = this.gameObject.GetComponent<MMHealthBar>();
        CurrentHealth = InitialHealth;
        DamageEnabled();
        UpdateHealthBar(false);
        _initialized = true;
    }

    /// <summary>
    /// Called when the object takes damage
    /// </summary>
    /// <param name="damage">The amount of health points that will get lost.</param>
    /// <param name="instigator">The object that caused the damage.</param>
    /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage.</param>
    /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
    public virtual void Damage(float damage)
    {
        if (damage <= 0)
        {
            OnHitZero?.Invoke();
            return;
        }

        // if the object is invulnerable, we do nothing and exit
        if (Invulnerable)
        {
            OnHitZero?.Invoke();
            return;
        }

        if (!this.enabled)
        {
            return;
        }

        // if we're already below zero, we do nothing and exit
        if ((CurrentHealth <= 0) && (InitialHealth != 0))
        {
            return;
        }

        // we decrease the character's health by the damage
        CurrentHealth -= damage;

        if (_animationController != null)
        {
            _animationController.PlayAnim("Hit");
        }

        OnHit?.Invoke(damage);

        // we play the damage feedback
        DamageFeedbacks?.PlayFeedbacks(this.transform.position);

        // we update the health bar
        UpdateHealthBar(true);

        // if health has reached zero
        if (CurrentHealth <= 0)
        {
            // we set its health to zero (useful for the healthbar)
            CurrentHealth = 0;
            Kill();
        }
    }

    /// <summary>
    /// Called when the character gets health (from a stimpack for example)
    /// </summary>
    /// <param name="health">The health the character gets.</param>
    /// <param name="instigator">The thing that gives the character health.</param>
    public virtual void GetHealth(int health, GameObject instigator)
    {
        // this function adds health to the character's Health and prevents it to go above MaxHealth.
        CurrentHealth = Mathf.Min(CurrentHealth + health, MaximumHealth);
        UpdateHealthBar(true);
    }

    /// <summary>
    /// Sets the health of the character to the one specified in parameters
    /// </summary>
    /// <param name="newHealth"></param>
    /// <param name="instigator"></param>
    public virtual void SetHealth(int newHealth, GameObject instigator)
    {
        CurrentHealth = Mathf.Min(newHealth, MaximumHealth);
        UpdateHealthBar(false);
    }

    /// <summary>
    /// Resets the character's health to its max value
    /// </summary>
    public virtual void ResetHealthToMaxHealth()
    {
        CurrentHealth = MaximumHealth;
        UpdateHealthBar(false);
    }

    /// <summary>
    /// Updates the character's health bar progress.
    /// </summary>
    public virtual void UpdateHealthBar(bool show)
    {
        if (_healthBar != null)
        {
            _healthBar.UpdateBar(CurrentHealth, 0f, MaximumHealth, show);
        }
    }

    /// <summary>
    /// Kills the character, instantiates death effects, handles points, etc
    /// </summary>
    public virtual void Kill()
    {
        // we prevent further damage
        DamageDisabled();

        // instantiates the destroy effect
        DeathFeedbacks?.PlayFeedbacks();

        if (_animationController != null)
        {
            _animationController.PlayAnim("Death");
        }

        // we make it ignore the collisions from now on
        if (CollisionsOffOnDeath)
        {
            if (_collider2D != null)
            {
                _collider2D.enabled = false;
            }
        }

        if (DelayBeforeDestruction > 0f)
        {
            Invoke("DestroyObject", DelayBeforeDestruction);
        }
        else
        {
            // finally we destroy the object
            DestroyObject();
        }

        OnDeath?.Invoke();
    }

    /// <summary>
    /// Destroys the object, or tries to, depending on the character's settings
    /// </summary>
    protected virtual void DestroyObject()
    {
        if (DestroyOnDeath)
        {
            gameObject.SetActive(false);
            return;
        }

        if (_autoRespawn == null)
        {
            // object is turned inactive to be able to reinstate it at respawn
            gameObject.SetActive(false);
        }
        else
        {
            _autoRespawn.Kill();
        }
    }

    /// <summary>
    /// Revive this object.
    /// </summary>
    public virtual void Revive()
    {
        if (!_initialized)
        {
            return;
        }

        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }

        Init();

        UpdateHealthBar(false);
        OnRevive?.Invoke();
    }

    /// <summary>
    /// Prevents the character from taking any damage
    /// </summary>
    public virtual void DamageDisabled()
    {
        Invulnerable = true;
    }

    /// <summary>
    /// Allows the character to take damage
    /// </summary>
    public virtual void DamageEnabled()
    {
        Invulnerable = false;
    }
}