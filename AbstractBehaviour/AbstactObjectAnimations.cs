using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;

namespace AbstractBehaviour
{
    /// <summary>
    /// Структура для быстрого доступа к состояниям
    /// </summary>
    public struct StructStatesNames
    {
        private List<string> states; // состояния

        /// <summary>
        /// Число анимаций в структуре
        /// </summary>
        public float StructureCount
        {
            get
            {
                if (states == null)
                    return -1;
                else
                    return states.Count;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parameters"></param>
        public StructStatesNames(Animator animator)
        {
            int count = animator.parameters.Length;
            states = new List<string>();
            for (int i = 0;i<count;i++)
                states.Add(animator.parameters[i].name);
        }

        /// <summary>
        /// Получить состояние по его номеру
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetState(byte state)
        {
            if (states == null)
                return null;
            else
                return states[state];
        }
    }

    /// <summary>
    /// Абстрактным образом описывает анимацию для любого игрового объекта.
    /// </summary>
    public abstract class AbstactObjectAnimations 
        : MonoBehaviour, IVotanObjectAnimations
    {
        #region Переменные
        [SerializeField, Tooltip("Аниматор объекта")]
        protected Animator animatorOfObject;
        protected StructStatesNames structStatesNames;
        [SerializeField]
        private Transform transformForDeadYNormalizing;
        private bool isDowner;
        #endregion

        #region Свойства
        public Animator AnimatorOfObject
        {
            get
            {
                return animatorOfObject;
            }

            set
            {
                animatorOfObject = value;
            }
        }

        public StructStatesNames StructStatesNames
        {
            get
            {
                return structStatesNames;
            }

            set
            {
                structStatesNames = value;
            }
        }

        protected Transform TransformForDeadYNormalizing
        {
            get
            {
                return transformForDeadYNormalizing;
            }

            set
            {
                transformForDeadYNormalizing = value;
            }
        }

        public bool IsDowner
        {
            get
            {
                return isDowner;
            }

            set
            {
                isDowner = value;
            }
        }
        #endregion

        /// <summary>
        /// Установить значение состояния
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public virtual void SetState(byte state, bool flag)
        {
            animatorOfObject.SetBool
                (StructStatesNames.GetState(state), flag);
        }

        /// <summary>
        /// Плучить булево значение состояния
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool GetBoolFromState(byte state)
        {
            return animatorOfObject.
                GetBool(structStatesNames.GetState(state));
        }

        /// <summary>
        /// Низкая скорость анимации
        /// </summary>
        public abstract void LowSpeedAnimation();

        /// <summary>
        /// Высокая скорость анимации
        /// </summary>
        public abstract void HighSpeedAnimation();

        /// <summary>
        /// Все ли состояния выключены?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFalseAllStates()
        {
            for (byte i = 0; i < structStatesNames.StructureCount; i++)
                if (GetBoolFromState(i)) return false;

            return true;
        }

        /// <summary>
        /// Получить скорость аниматора
        /// </summary>
        /// <returns></returns>
        public float GetAnimatorSpeed()
        {
            return animatorOfObject.speed;
        }

        /// <summary>
        /// Своя скорость анимации
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetSpeedAnimationByRunSpeed(float value)
        {
			animatorOfObject.speed = value;
        }

        /// <summary>
        /// Отключить все состояния анимации кроме последнего.
        /// По-умолчанию, последнее состояние считается за смерть
        /// </summary>
        public virtual void DisableAllStates()
        {
            if (structStatesNames.StructureCount == -1)
            {
                return;
            }
            else
            {
                for (byte i = 0; i < structStatesNames.StructureCount - 1; i++)
                {
                    if (animatorOfObject.gameObject.activeSelf)
                        animatorOfObject.SetBool(StructStatesNames.GetState(i), false);
                    else
                        return;
                }
            }
        }

        /// <summary>
        /// Проиграть корутину на нормализацию положения относительно
        /// координаты "Y"
        /// </summary>
        public abstract void PlayDeadNormalizeCoroutine();
            
        /// <summary>
        /// Корутина на нормализацию по координате "Y"
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<float> CoroutineDeadYNormalized();

        /// <summary>
        /// Установить минимальную скорость анимации без проверки на состояние
        /// </summary>
        public abstract void NonCheckLowSpeedAnimation();
    }
}
