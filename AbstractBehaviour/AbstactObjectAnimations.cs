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

        public float structureCount
        {
            get
            {
                return states.Count;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parameters"></param>
        public StructStatesNames(params string[] parameters)
        {
            states = new List<string>();
            foreach (string parameter in parameters)
                states.Add(parameter);
        }

        /// <summary>
        /// Получить состояние по его номеру
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetState(byte state)
        {
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
        protected StructStatesNames structStatesnames;
        [SerializeField]
        protected Transform transformForDeadYNormalizing;
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
                return structStatesnames;
            }

            set
            {
                structStatesnames = value;
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
                GetBool(structStatesnames.GetState(state));
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
            for (byte i = 0; i < structStatesnames.structureCount; i++)
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
            for (byte i = 0; i < structStatesnames.structureCount-1; i++)
                animatorOfObject.SetBool(StructStatesNames.GetState(i), false);
        }

        public abstract void PlayDeadNormalizeCoroutine();
            
        public abstract IEnumerator<float> CoroutineDeadYNormalized();
    }
}
