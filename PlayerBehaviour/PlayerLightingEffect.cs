using System;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует вспышку у игрока во время грозы.
    /// </summary>
    public class PlayerLightingEffect
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Изображение для грозы")]
        private Image image;
        private Color lightColor = new Color(1, 1, 1, 0.19f);
        private Color normalColor = new Color(1, 1, 1, 0);

        /// <summary>
        /// Запуск эффекта грозы
        /// </summary>
        public void FireLightingEffect()
        {
            Timing.RunCoroutine(CoroutineForLightingImageEffect());
        }

        /// <summary>
        /// Корутина для эффекта грозы
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLightingImageEffect()
        {
            image.gameObject.SetActive(true);
            image.color = lightColor;
            yield return Timing.WaitForSeconds(0.05f);
            image.color = normalColor;
            image.gameObject.SetActive(false);
        }
    }
}
