using UnityEngine;

public class ButtonSettingBase : MonoBehaviour
{
    public ButtonController switchBtn;
    
    protected EffectManager effectManager;

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void Awake()
    {
        effectManager = GetComponent<EffectManager>();
    }


    protected virtual void Start()
    {
        switchBtn.OnClick.AddListener(SwitchOnOff);
    }

    protected virtual void SwitchOnOff()
    {
    }

    protected virtual void ListenEvent(bool isOn)
    {
        ChangeState(isOn);
    }

    protected void ChangeState(bool isOn)
    {
        if (isOn)
        {
            effectManager.ShowEffect();
        }
        else
        {
            effectManager.HideEffect();
        }
    }
}