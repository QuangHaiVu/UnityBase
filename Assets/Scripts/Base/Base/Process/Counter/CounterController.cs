using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Unity.Base
{
    public class CounterController : ProcessController
    {
        [SerializeField] private bool isLoop;

        public override void ChangeValue(float value)
        {
            if (currentValue <= 0 && !isLoop) return;
            base.ChangeValue(value);
        }

        protected override void Complete()
        {
            base.Complete();
            if (isLoop) ResetValue();
        }
    }
}