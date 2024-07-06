using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Timer 
    {
        private float currentTime;
        private readonly float timePeriod;

        private readonly Action onTimerEnd;
        private readonly Action<float> onTimerTick;

        public bool IsPaused;

        public Timer(float _timePeriod, Action _onTimerEnds, Action<float> _onTimerTicks= null)
        {
            timePeriod = _timePeriod; 
            onTimerEnd = _onTimerEnds; 
            onTimerTick = _onTimerTicks;

            ResetCurrentTime();
            IsPaused = true;
        }

        public void Start() => IsPaused = false;

        public void Restart() 
        {
            ResetCurrentTime();
            Start();
        }

        public void Tick(float deltaTime)
        {
            if(IsPaused) { return; }
            currentTime += deltaTime;
            onTimerTick?.Invoke(currentTime);

            if(currentTime > timePeriod)
            {
                onTimerEnd();
            }
        }
        private void ResetCurrentTime()
        {
            currentTime = 0f;
        }
    }
}