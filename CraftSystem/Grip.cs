using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Ручка оружия
    /// </summary>
    public class Grip 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Range(1, 25f),Tooltip("Вес ручки")]
        private float gripWeight;
        [SerializeField, Range(1, 100f), Tooltip("Защита")]
        private float gripDefence;
        [SerializeField, Range(0, 75f), Tooltip("Бонус к скорости вращения за счет ручки")]
        private float bonusSpinSpeedFromGrip;

        [SerializeField, Range(0, 3)]
        private int gripType;
        [SerializeField]
        private string weaponName;

        #endregion

        #region Свойства 
        public string WeaponName
        {
            get
            {
                return weaponName;
            }

            set
            {
                weaponName = value;
            }
        }

        public int GripType
        {
            get
            {
                return gripType;
            }

            set
            {
                gripType = value;
            }
        }

        public float BonusSpinSpeedFromGrip
        {
            get
            {
                return bonusSpinSpeedFromGrip;
            }

            set
            {
                bonusSpinSpeedFromGrip = value;
            }
        }

        public float GripWeight
        {
            get
            {
                return gripWeight;
            }

            set
            {
                gripWeight = value;
            }
        }

        public float GripDefence
        {
            get
            {
                return gripDefence;
            }

            set
            {
                gripDefence = value;
            }
        }
        #endregion
    }
}
