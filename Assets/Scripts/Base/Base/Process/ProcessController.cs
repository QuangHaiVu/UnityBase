using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TheLegends.Unity.Base
{
    public class ProcessController : MonoBehaviour
    {
        [SerializeField] protected float maxValue;
        protected float currentValue;
        [SerializeField] private UnityEvent<float> onChange;
        [SerializeField] private UnityEvent onComplete;

        public float CurrentValue
        {
            get => currentValue;
            set => currentValue = value;
        }

        public UnityEvent<float> OnChange
        {
            get => onChange;
            set => onChange = value;
        }

        public UnityEvent OnComplete
        {
            get => onComplete;
            set => onComplete = value;
        }



        protected virtual void Start()
        {
            ResetValue();
        }

        public void SetMaxValue(float value)
        {
            maxValue = value;
        }

        public void ResetValue()
        {
            currentValue = maxValue;
        }

        public void SetCurrentValue(float value)
        {
            currentValue = value;
            ChangeValue(0);
        }

        public virtual void ChangeValue(float value)
        {
            if (currentValue <= 0) return;
            if (currentValue >= maxValue) currentValue = maxValue;

            currentValue -= value;
            onChange?.Invoke(currentValue);

            if (currentValue <= 0)
            {
                Complete();
            }
        }

        protected virtual void Complete()
        {
            onComplete?.Invoke();
        }
    }
}
