using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        [Header("Durations (seconds)")]
        public float dayDuration = 30f;
        public float nightDuration = 15f;

        [Header("Color Over Full Day")]
        public Gradient dayNightGradient;

        [Header("Light Intensity")]
        public float nightIntensity = 0.3f;
        public float dayIntensity = 1f;

        [Header("Sunrise / Sunset")]
        [Tooltip("Portion of day used for sunrise and sunset (0–0.5)")]
        [Range(0.05f, 0.4f)]
        public float transitionPortion = 0.2f;

        public int currentDay = 1;

        public delegate void DayStarted(int day);
        public static event DayStarted OnDayStarted;

        private Light2D _light;
        private float _timer;
        private bool _isDay = true;

        private float TotalCycleDuration => dayDuration + nightDuration;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > TotalCycleDuration)
            {
                _timer -= TotalCycleDuration;
                _isDay = true;
                currentDay++;
                OnDayStarted?.Invoke(currentDay);
            }
            

            if (_timer > dayDuration && _isDay)
            {
                _isDay = false;
                // Night start logic here
                CheckVictory();
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

        private void CheckVictory()
        {
            DamCollecting dam = FindFirstObjectByType<DamCollecting>();

            if (dam != null && dam.collectedCount >= dam.requiredWood)
            {
                Debug.Log("Victory!");
            }
            else
            {
                Debug.Log("Failed the day!");
            }
        }
        private float GetColorTime()
        {
            if (_timer <= dayDuration)
                return _timer / dayDuration;

            return 1f; // night color
        }

        /// <summary>
        /// Smooth intensity fade in sunrise and sunset.
        /// </summary>
        private float GetIntensityTime()
        {
            float sunriseEnd = dayDuration * transitionPortion;
            float sunsetStart = dayDuration * (1f - transitionPortion);

            if (_timer < sunriseEnd)
            {
                // Sunrise fade in
                return Mathf.InverseLerp(0f, sunriseEnd, _timer);
            }

            if (_timer > sunsetStart && _timer <= dayDuration)
            {
                // Sunset fade out
                return Mathf.InverseLerp(dayDuration, sunsetStart, _timer);
            }

            if (_timer <= dayDuration)
            {
                // Full daylight
                return 1f;
            }

            // Night
            return 0f;
        }
    }
}
