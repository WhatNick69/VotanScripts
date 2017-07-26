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
        [SerializeField, Tooltip("Эта часть будет уничтожаться?")]
        private bool isActivePart;
        [SerializeField, Tooltip("Номер позиции")]
        private ArmoryPosition numberPosition;

        private Rigidbody rigFromObj;
        private BoxCollider boxCollider;
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
        private void Awake()
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

        /// <summary>
        /// Зажечь событие. 
        /// Часть брони отрывается от тела и отскакивает.
        /// </summary>
        public void FireEvent()
        {
            if (!isActivePart) return;

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
            //float localScale = 
            //  transform.localScale.x;
            //transform.localScale = 
            //  new Vector3(localScale, localScale, localScale);
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
