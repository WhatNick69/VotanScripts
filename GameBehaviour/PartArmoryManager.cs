using PlayerBehaviour;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Контроль над частью брони
    /// </summary>
    public class PartArmoryManager
        : ArmoryObject
    {
        #region Переменные
        [SerializeField, Tooltip("Номер позиции")]
        private ArmoryPosition numberPosition;
        #endregion

        #region Свойства
        public bool IsActivePart
        {
            get
            {
                return isActivePart;
            }

            set
            {
                isActivePart = value;
            }
        }

        public ArmoryPosition NumberPosition
        {
            get
            {
                return numberPosition;
            }

            set
            {
                numberPosition = value;
            }
        }
        #endregion

        /// <summary>
        /// <summary>
        /// Инициализация 2
        /// </summary>
        protected override void Awake()
        {
            transform.SetParent(GameObject.FindWithTag("Player").
                GetComponent<PlayerComponentsControl>().PlayerArmory.
                GetMyParent((int)numberPosition));
            transform.localPosition = Vector3.zero;

            if (!isActivePart) return;
            rigFromObj = transform.GetComponentInChildren<Rigidbody>();
            boxCollider = transform.GetComponentInChildren<BoxCollider>();
            rigFromObj.detectCollisions = false;
        }
    }
}
