using System.Collections.Generic;
using UnityEngine;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактным образом описывает анимацию для любого игрового объекта.
    /// Пожалуй, это лучший класс во всем проекте, ибо он красив
    /// </summary>
    public abstract class AbstactObjectAnimations 
        : MonoBehaviour
    {
        /// <summary>
        /// Структура для быстрого доступа к состояниям
        /// </summary>
        protected struct StructStatesNames
        {
            private List<string> states;

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

        #region Переменные
        [SerializeField, Tooltip("Аниматор объекта")]
        protected Animator animatorOfObject;
        protected StructStatesNames structStatesNames;
        #endregion

        /// <summary>
        /// Установить значение состояния
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flag"></param>
        public virtual void SetState(byte state, bool flag)
        {
            animatorOfObject.SetBool
                (structStatesNames.GetState(state), flag);
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
        public void SetSpeedAnimationByRunSpeed(float value)
        {
            Debug.Log(value);
            animatorOfObject.speed = value;
        }

        /// <summary>
        /// Отключить все состояния анимации кроме последнего.
        /// По-умолчанию, последнее состояние считается за смерть
        /// </summary>
        public virtual void DisableAllStates()
        {
            for (byte i = 0; i < 4; i++)
                animatorOfObject.SetBool(structStatesNames.GetState(i), false);
        }
    }
}
