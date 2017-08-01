using UnityEngine;
using UnityEngine.UI;

namespace VotanUI
{
    /// <summary>
    /// Настройки
    /// </summary>
    public class Settings 
        : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        /// <summary>
        /// Обновление с заданной частотой
        /// </summary>
        private void FixedUpdate()
        {
            AudioListener.volume = slider.value;
        }
    }
}
