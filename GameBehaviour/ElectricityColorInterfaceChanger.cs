using System;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Компонента, которая реализовывает эффект осветления тени
    /// при получении электрического урона врагом/игроком
    /// </summary>
    public class ElectricityColorInterfaceChanger 
        : MonoBehaviour
    {
        private SpriteRenderer spriteRendererObject;
        private byte counter;
        private Color whiteColor;
        private Color blackColor;

        public SpriteRenderer SpriteRendererObject
        {
            get
            {
                return spriteRendererObject;
            }

            set
            {
                spriteRendererObject = value;
            }
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            whiteColor = new Color(1, 1, 1, 0.58f);
            blackColor = new Color(0, 0, 0, 0.58f);
        }

        /// <summary>
        /// Зажечь эффект осветления тени
        /// </summary>
        public void ElectricityBlobShadow()
        {
            Timing.RunCoroutine(CoroutineForElectricityBlobShadow());
        }

        /// <summary>
        /// Корутина на осветление/затемнение тени
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForElectricityBlobShadow()
        {
            while (counter < 10)
            {
                spriteRendererObject.color =
                    Color.Lerp(spriteRendererObject.color, whiteColor, Time.deltaTime * 10);
                counter++;
                yield return Timing.WaitForSeconds(0.05f);
            }
            spriteRendererObject.color = whiteColor;
            counter = 0;

            while (counter < 10)
            {
                spriteRendererObject.color =
                    Color.Lerp(spriteRendererObject.color, blackColor, Time.deltaTime * 10);
                counter++;
                yield return Timing.WaitForSeconds(0.05f);
            }
            spriteRendererObject.color = blackColor;
            counter = 0;
        }
    }
}
