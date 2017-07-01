using System.Collections.Generic;
using UnityEngine;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактным образом описывает анимацию для любого игрового объекта
    /// </summary>
    public abstract class AbstactObjectAnimations 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Аниматор объекта")]
        private Animator animatorOfObject;

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
        #endregion


    }
}
