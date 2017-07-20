using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализует эффект кровавого затемнения 
    /// экрана при ударе по игроку
    /// </summary>
    public class PlayerBloodInterfaceEffect 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Изображение крови на экране")]
        private RectTransform bloodImage;
        private Image image;
        private PlayerComponentsControl playerComponentsControl;
        private bool isHiting;
        private bool isMaximum;
        private Color transparencyColor;
        private Color whiteColor;
        #endregion

        /// <summary>
        /// Инициализация 
        /// </summary>
        private void Start() // -----------------главный метод-----------------
        {
            transparencyColor = new Color(1, 1, 1, 0);
            whiteColor = new Color(1, 1, 1, 1f);
            playerComponentsControl = GetComponent<PlayerComponentsControl>();
            image = bloodImage.GetComponent<Image>();
        }

        /// <summary>
        /// Обновление
        /// </summary>
        private void Update()
        {
            LerpBloodImageAlpha();
        }

        /// <summary>
        /// Эффект кровавого затемнение при получении урона
        /// </summary>
        public void EventBloodEyesEffect(float frequentHP)
        {
            bloodImage.localScale = new Vector3(
                LibraryStaticFunctions.GetValueByFrequent(1,3,frequentHP),
                LibraryStaticFunctions.GetValueByFrequent(1, 3, frequentHP), 
                LibraryStaticFunctions.GetValueByFrequent(1, 3, frequentHP));
            isHiting = true;
        }


        /// <summary>
        /// Интерполяция альфы изображения
        /// </summary>
        private void LerpBloodImageAlpha()
        {
            if (isHiting && !isMaximum)
            {
                image.color = Color.LerpUnclamped(image.color, whiteColor, Time.deltaTime*10);
                if (image.color.a >= 0.95f)
                {
                    Debug.Log("MAX_HIT");
                    isMaximum = true;
                }
            }
            else if (isMaximum && isHiting)
            {
                image.color = Color.LerpUnclamped(image.color, transparencyColor, Time.deltaTime*5);
                if (image.color.a <= 0.05f)
                {
                    image.color = transparencyColor;
                    isMaximum = false;
                    isHiting = false;
                }
            }
        }
    }
}
