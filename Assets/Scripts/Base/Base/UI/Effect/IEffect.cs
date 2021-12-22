using System;

public interface IEffect
{
    void ShowEffect(Action showComplete = null);
    void HideEffect(Action endComplete = null);
    void Disable();
}