using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Контроль над частью брони
    /// </summary>
    public class PartArmoryManager
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Величина брони")]
        private float armoryValue;
        [SerializeField, Tooltip("Тип брони")]
        private ArmoryType armoryType;
        [SerializeField, Tooltip("Эта часть является главной? Если да - "
            +"то она не будет уничтожаться при дестркукции брони")]
        private bool isMainPartInArray;
        [SerializeField, Tooltip("Номер позиции"),Range(0,8)]
        private int numberPosition;

        private Rigidbody rigFromObj;
        private PlayerComponentsControl playerComponentsControl;
        private BoxCollider boxCollider;
        #endregion

        #region Свойства
        public bool IsMainPartInArray
        {
            get
            {
                return isMainPartInArray;
            }

            set
            {
                isMainPartInArray = value;
            }
        }
        #endregion

        /// <summary>
        /// Суммировать броню к общей броне игрока
        /// </summary>
        private void IncremenentToPlayerArmory()
        {
            if (armoryValue > 0)
                playerComponentsControl.PlayerArmory.HealthValue
                    += armoryValue;
        }

        /// <summary>
        /// Инициализация 2
        /// </summary>
        private void Start() 
        {
            playerComponentsControl =
                GameObject.FindWithTag("Player").GetComponent<PlayerComponentsControl>();
            transform.SetParent(playerComponentsControl.PlayerArmory.GetMyParent(numberPosition));
            transform.localPosition = Vector3.zero;
            IncremenentToPlayerArmory();
            InitToPlayerArmory();

            if (isMainPartInArray) return;
            rigFromObj = transform.GetComponentInChildren<Rigidbody>();
            boxCollider = transform.GetComponentInChildren<BoxCollider>();
            rigFromObj.detectCollisions = false;
        }

        /// <summary>
        /// Инициализация массивов брони для игрока
        /// </summary>
        private void InitToPlayerArmory()
        {
            switch (armoryType)
            {
                case ArmoryType.Helmet:
                    playerComponentsControl.PlayerArmory.Helmet 
                        = this;
                    break;
                case ArmoryType.Cuirass:
                    playerComponentsControl.PlayerArmory.KirasaParts.Add(this);
                    break;
                case ArmoryType.Shield:
                    playerComponentsControl.PlayerArmory.ShieldParts.Add(this);
                    break;
            }
        }

        /// <summary>
        /// Зажечь событие. 
        /// Часть брони отрывается от тела и отскакивает.
        /// </summary>
        public void FireEvent()
        {
            if (IsMainPartInArray) return;

            DetachFromParent();
            AddForceToObject();
            Timing.RunCoroutine(CoroutineForDisableObject());
        }

        /// <summary>
        /// Отсоединить от родителя
        /// </summary>
        private void DetachFromParent()
        {
            transform.parent = null;
        }

        /// <summary>
        /// Добавить физическую силу объекту
        /// </summary>
        private void AddForceToObject()
        {
            rigFromObj.useGravity = true;
            rigFromObj.detectCollisions = true;
            rigFromObj.isKinematic = false;
            rigFromObj.constraints = RigidbodyConstraints.None;
            rigFromObj.AddForce(new Vector3(LibraryStaticFunctions.GetPlusMinusValue(75),
                LibraryStaticFunctions.rnd.Next(40, 100),
                LibraryStaticFunctions.GetPlusMinusValue(75)));
        }

        /// <summary>
        /// Корутина на отключение объекта
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForDisableObject()
        {
            yield return Timing.WaitForSeconds(0.3f);
            boxCollider.enabled = true;
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.rnd.Next(5, 10));
            gameObject.SetActive(false);
        }
    }
}
