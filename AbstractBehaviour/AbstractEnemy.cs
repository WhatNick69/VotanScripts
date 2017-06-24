using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный враг
    /// </summary>
    public abstract class AbstractEnemy
      : MonoBehaviour
    {
        [SerializeField]
        Transform startGunPoint, finishGunPoint,
            rightShoulderPoint, leftShoulderPoint,
            facePoint, backPoint;

        [SerializeField,Tooltip("Здоровье")]
        private float health = 1;

        public float Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        // Возвращает положение врага или его точек в сцене
        public Vector3 ReturnPosition(int Child)
        {
            if (Child == 0) return rightShoulderPoint.position; //Right
            if (Child == 1) return leftShoulderPoint.position; //Left
            if (Child == 2) return facePoint.position; //Face
            if (Child == 3) return backPoint.position; //Back
            if (Child == 4) return transform.position; // Позиция врага
            else return Vector3.zero;
        }

        /// <summary>
        /// Умереть
        /// </summary>
        public void Die()
        {
            Destroy(gameObject, 0.05f);
            //Timing.RunCoroutine(CoroutineDie());
        }

        /// <summary>
        /// Корутин
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineDie()
        {
            yield return Timing.WaitForSeconds(0.05f);

        }

        /// <summary>
        /// Вернуть здоровье
        /// </summary>
        /// <returns></returns>
        public float ReturnHealth()
        {
            return Health;
        }

        /// <summary>
        /// Получить урон
        /// </summary>
        /// <param name="dmg"></param>
        public void GetDamage(float dmg)
        {
            Health -= dmg;
            if (Health <= 0) Die();
        }
    }
}
