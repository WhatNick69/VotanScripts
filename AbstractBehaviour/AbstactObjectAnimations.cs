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
        [SerializeField, Tooltip("Анимация персонажа")]
        protected Animation animationOfPlayer;
        [SerializeField, Tooltip("Лист векторов")]
        protected List<Vector2> animationsStateList = new List<Vector2>();
        protected string animationName;
        protected float frameLenght;
        #endregion 


        // запилить в Animator переменную, отвечающую за скорость анимации


        /// <summary>
        /// Инициализация клипа анимации
        /// </summary>
        protected void InitialisationAnimationClipName()
        {
            foreach (AnimationState clip in animationOfPlayer)
            {
                animationName = clip.name;
                frameLenght = 1/(animationOfPlayer[animationName].length 
                    * animationOfPlayer[animationName].clip.frameRate);
                break;
            }
        }

        /// <summary>
        /// Получить вектор анимации из листа по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Vector2 GetAnimationPartByIndex(int index)
        {
            return animationsStateList[index];
        }
    }
}
