using UnityEngine;
using VotanLibraries;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный класс для реализации любого врага
    /// </summary>
    public abstract class AbstractEnemy
      : MonoBehaviour
    {
		[SerializeField]
		protected Transform rightShoulderPoint, leftShoulderPoint, // точки противника
			facePoint, backPoint;
        [SerializeField,Tooltip("Аниматор врага")]
        public Animator enAnim;
        // ссылка на компонент, реализующий состояние врага
        [SerializeField]
        private AbstractObjectConditions absObjCond;
        [SerializeField]
        private AbstractAttack absAttack;

        public AbstractAttack AbsAttack
        {
            get
            {
                return absAttack;
            }

            set
            {
                absAttack = value;
            }
        }

        public AbstractObjectConditions AbsObjCond
        {
            get
            {
                return absObjCond;
            }

            set
            {
                absObjCond = value;
            }
        }

        /// <summary>
        /// Возвращает положение врага или его точек в сцене
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Vector3 ReturnPosition(int child)
        {
            switch (child)
            {
                case 0:
                    return rightShoulderPoint.position; //Right
                case 1:
                    return leftShoulderPoint.position; //Left
                case 2:
                    return facePoint.position; //Face
                case 3:
                    return backPoint.position; //Back
                case 4:
                    return transform.position; // Позиция врага
                case 5:
                    return absAttack.PlayerStartGunPoint.position;
                case 6:
                    return absAttack.PlayerFinishGunPoint.position;
            }  
			return Vector3.zero;
        }

        /// <summary>
        /// Вернуть здоровье
        /// </summary>
        /// <returns></returns>
        public virtual float ReturnHealth()
        {
            return absObjCond.HealthValue;
        }

        /// <summary>
        /// Получить урон
        /// </summary>
        /// <param name="dmg"></param>
        public virtual void GetDamage(float dmg)
        {
            absObjCond.HealthValue -= LibraryStaticFunctions.GetPlusMinusDmg(dmg, 0.1f);
        }
	}
}
