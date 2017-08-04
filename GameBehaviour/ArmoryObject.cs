using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Описывает типичное поведение части брони 
    /// как для игрока, так для и врага
    /// </summary>
    public class ArmoryObject 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Эта часть будет уничтожаться?")]
        protected bool isActivePart;
        protected Rigidbody rigFromObj;
        protected BoxCollider boxCollider;

        /// <summary>
        /// Инициализация
        /// </summary>
        protected virtual void Awake()
        {
            if (!isActivePart) return;
            rigFromObj = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            rigFromObj.detectCollisions = false;
        }

        /// <summary>
        /// Зажечь событие
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
        protected virtual void DetachFromParent()
        {
            transform.parent = null;
            float localScale =
              transform.localScale.x;
            transform.localScale =
              new Vector3(localScale, localScale, localScale);
        }

        /// <summary>
        /// Добавить физическую силу объекту
        /// </summary>
        protected virtual void AddForceToObject()
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
        protected virtual IEnumerator<float> CoroutineForDisableObject()
        {
            yield return Timing.WaitForSeconds(0.3f);
            boxCollider.enabled = true;
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.rnd.Next(5, 10));
            gameObject.SetActive(false);
        }
    }
}
