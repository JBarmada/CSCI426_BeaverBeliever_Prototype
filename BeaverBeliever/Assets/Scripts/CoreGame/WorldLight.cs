using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        public float dayDuration = 30f;
        public float nightDuration = 15f;
        public Gradient dayNightGradient;
        public float nightIntensity = 0.3f;
        public float dayIntensity = 1f;
        
        [Range(0.05f, 0.4f)]
        public float transitionPortion = 0.2f;

        // Event for Night Start
        public static event Action OnNightStart;
        public static event Action OnDayCycleEnd; 

        private Light2D _light;
        private float _timer;
        private bool _isNightTriggered = false; // Prevent double trigger
        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        public void ResetDay()
        {
            _timer = 0;
            _isNightTriggered = false;
            UpdateLighting();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            // Trigger Night Event once
            if (_timer > dayDuration && !_isNightTriggered)
            {
                _isNightTriggered = true;
                OnNightStart?.Invoke();
            }

            // End of Cycle
            if (_timer > (dayDuration + nightDuration))
            {
                _timer = 0; 
                _isNightTriggered = false;
                OnDayCycleEnd?.Invoke(); 
            }

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            float colorT = GetColorTime();
            float intensityT = GetIntensityTime();

            _light.color = dayNightGradient.Evaluate(colorT);
            _light.intensity = Mathf.Lerp(nightIntensity, dayIntensity, intensityT);
        }

        private float GetColorTime()
        {
            if (_timer <= dayDuration) return _timer / dayDuration;
            return 1f; 
        }

        private float GetIntensityTime()
        {
            float sunriseEnd = dayDuration * transitionPortion;
            float sunsetStart = dayDuration * (1f - transitionPortion);

            if (_timer < sunriseEnd) return Mathf.InverseLerp(0f, sunriseEnd, _timer);
            if (_timer > sunsetStart && _timer <= dayDuration) return Mathf.InverseLerp(dayDuration, sunsetStart, _timer);
            if (_timer <= dayDuration) return 1f;
            return 0f;
        }
    }
}