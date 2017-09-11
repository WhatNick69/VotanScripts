using UnityEngine;
using UnityEngine.UI;

namespace VotanUI
{
    /// <summary>
    /// Настройки. Пока что только звук.
    /// </summary>
    public class Settings 
        : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        /// <summary>
        /// Изменить громкость
        /// </summary>
        public void ChangeVolume()
        {
            AudioListener.volume = slider.value;
        }
    }
}
