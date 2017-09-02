using UnityEngine;
using VotanInterfaces;

namespace EnemyBehaviour
{
    /// <summary>
    /// Арбалет врага "Арбалетчик"
    /// </summary>
    public class CrossbowWeapon
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Аммуниция арбалета")]
        private Transform ammunation;
        [SerializeField, Tooltip("Скорость движения болта"), Range(1, 100)]
        private float moveSpeed;
        [SerializeField, Tooltip("Время жизни болта"), Range(1, 10)]
        private float timeToRestart;
        private float dmgValue;

        private Transform[] ammunationStack;
        private IAmmunation[] ammunationInterfaces;
        private IPlayerBehaviour playerComponentsControl;

        public IPlayerBehaviour PlayerComponentsControl
        {
            get
            {
                return playerComponentsControl;
            }

            set
            {
                playerComponentsControl = value;
            }
        }

        public float DmgValue
        {
            get
            {
                return dmgValue;
            }

            set
            {
                dmgValue = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            ammunationStack = new Transform[ammunation.childCount];
            ammunationInterfaces = new IAmmunation[ammunation.childCount];
            for (int i = 0;i<ammunation.childCount;i++)
            {
                ammunationStack[i] = ammunation.GetChild(i).transform;
                ammunationInterfaces[i] = ammunation.GetChild(i).GetComponent<IAmmunation>();
                ammunationInterfaces[i].InitialisationAmmunationElement();
            }
        }

        /// <summary>
        /// Стрелять
        /// </summary>
        public void Fire()
        {
            for (int i = 0;i<ammunationInterfaces.Length;i++)
            {
                if (ammunationInterfaces[i].IsRestarted)
                {
                    ammunationInterfaces[i].FireAmmoObject(playerComponentsControl, 
                        dmgValue,moveSpeed, timeToRestart);
                    break;
                }
            }
        }

        /// <summary>
        /// Перезарядка оружия
        /// </summary>
        public bool Reload()
        {
            for (int i = 0; i < ammunationInterfaces.Length; i++)
            {
                if (ammunationInterfaces[i].IsRestarted)
                {
                    ammunationStack[i].gameObject.SetActive(true);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Не пустая ли аммуниция
        /// </summary>
        /// <returns></returns>
        public bool IsNotEmptyAmmunation()
        {
            for (int i = 0; i < ammunationInterfaces.Length; i++)
            {
                if (ammunationInterfaces[i].IsRestarted)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
