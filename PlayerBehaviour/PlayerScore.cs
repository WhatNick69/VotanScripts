using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализовывает возможность игроку вести счет очков
    /// </summary>
    public class PlayerScore 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Количество очков")]
        private int scoreValue;
        [SerializeField, Tooltip("Элемент UI - Text")]
        private Text textElement;

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
        /// Добавить очки
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> AddNewScore()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(2);
                AddScore(Random.Range(23, 1000));
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
                if (tempScore / 5 >= 1)
                {
                    tempScore -= 5;
                    scoreValue += 5;
                }
                else
                {
                    tempScore--;
                    scoreValue++ ;
                }
                textElement.text = scoreValue.ToString();
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
