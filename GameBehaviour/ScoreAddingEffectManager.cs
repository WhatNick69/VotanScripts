using System.Collections.Generic;
using PlayerBehaviour;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Эффект трэилов, что при достижении персонажа 
    /// отдают ему бонус за убийство врага
    /// </summary>
    public class ScoreAddingEffectManager
        : MonoBehaviour, IScoreAddingEffect
    {
        #region Переменные
        [SerializeField, Tooltip("Бонус за убийство")]
        private int scoreBonus;
        [SerializeField, Tooltip("Трэил-бонус")]
        private Transform trailBonus;
        [SerializeField, Tooltip("Частота обновления")]
        private float updateFrequency;

        private Transform playerTransform;
        private PlayerScore playerScore;
        #endregion

        #region Свойства
        public int ScoreBonus
        {
            get
            {
                return scoreBonus;
            }
        }

        public Transform TrailScore
        {
            get
            {
                return trailBonus;
            }

            set
            {
                trailBonus = value;
            }
        }
        #endregion

        /// <summary>
        /// Зажечь событие бонуса
        /// </summary>
        /// <param name="weapon"></param>
        public void EventEffect(IWeapon weapon)
        {
            scoreBonus = (int)LibraryStaticFunctions.GetRangeValue(scoreBonus, 0.1f);
            playerTransform = weapon.GetPlayer.PlayerWeapon.transform;
            playerScore = weapon.GetPlayer.PlayerScore;
            trailBonus.GetComponent<TrailRenderer>().startColor = weapon.TrailRenderer.startColor;
            trailBonus.GetComponent<TrailRenderer>().endColor = weapon.TrailRenderer.startColor;
            Timing.RunCoroutine(CoroutineForSearchPlayer());
        }

        /// <summary>
        /// Корутина преследования игрока
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSearchPlayer()
        {
            trailBonus.gameObject.SetActive(true);
            //trailBonus.SetParent(null);
            while (Vector3.Distance(trailBonus.position, playerTransform.position) >= 0.5f)
            {
                trailBonus.LookAt(playerTransform);
                trailBonus.Translate(trailBonus.forward,Space.World);
                yield return Timing.WaitForSeconds(updateFrequency);
            }
            playerScore.AddScore(scoreBonus);
            trailBonus.gameObject.SetActive(false);
        }
    }
}
