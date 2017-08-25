using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerBehaviour
{
    public class PlayerResources
        : MonoBehaviour
    {
        #region Переменные
        private int scoreValue;
        [SerializeField, Tooltip("Элемент UI - Text")]
        private Text textElement;

        private long woodResource;
        private long steelResource;
        private long gems;
        #endregion

        #region Свойства
        public long Gems
        {
            get
            {
                return gems;
            }

            set
            {
                gems = value;
            }
        }

        public long SteelResource
        {
            get
            {
                return steelResource;
            }

            set
            {
                steelResource = value;
            }
        }

        public long WoodResource
        {
            get
            {
                return woodResource;
            }

            set
            {
                woodResource = value;
            }
        }

        public int ScoreValue
        {
            get
            {
                return scoreValue;
            }

            set
            {
                scoreValue = value;
            }
        }
        #endregion

        /// <summary>
        /// Корутина для плавного зачисления очков на счет игроку
        /// </summary>
        /// <param name="addScoreValue"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSlowmotionAddingScore(int addScoreValue)
        {
            int tempScore = addScoreValue;
            while (tempScore > 0)
            {
                if (tempScore / 10 >= 1)
                {
                    tempScore -= 10;
                    scoreValue += 10;
                }
                else
                {
                    tempScore--;
                    scoreValue++;
                }
                textElement.text = scoreValue.ToString();
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Добавить очки игроку
        /// </summary>
        /// <param name="addScoreValue">Величина очков</param>
        public void AddScore(int addScoreValue)
        {
            Timing.RunCoroutine(CoroutineForSlowmotionAddingScore(addScoreValue));
        }
    }
}
