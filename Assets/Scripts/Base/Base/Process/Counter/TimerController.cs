using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Unity.Base
{
    public class TimerController : CounterController
    {
        [SerializeField] private UnityEvent onTimerStart;
        [SerializeField] private bool isAutoStart = false;
        private bool isActive = false;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        protected override void Start()
        {
            base.Start();
            if (isAutoStart) IsActive = true;
        }

        private void LateUpdate()
        {
            if (!isActive) return;
            if (currentValue == maxValue)
            {
                onTimerStart?.Invoke();
            }

            base.ChangeValue(Time.deltaTime);
        }
    }
}