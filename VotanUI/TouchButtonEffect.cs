using CraftSystem;
using UnityEngine;
using UnityEngine.UI;

namespace VotanUI
{
    /// <summary>
    /// Эффект свечения кнопки, которая была нажата
    /// </summary>
    public class TouchButtonEffect
        : MonoBehaviour, IRepositoryObject
    {
        [SerializeField]
        private Image imageTouch;

        /// <summary>
        /// Эффект подсветки
        /// </summary>
        /// <param name="flag"></param>
        public void HighlightingControl(bool flag)
        {
            if (flag)
            {
                imageTouch.enabled = true;
            }
            else
            {
                imageTouch.enabled = false;
            }
        }
    }
}
