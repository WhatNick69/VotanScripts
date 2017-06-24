using System.Collections.Generic;
using UnityEngine;

namespace VotanObjectBehaviour
{
    /// <summary>
    /// Абстрактным образом описывает анимацию для любого игрового объекта
    /// </summary>
    public abstract class AbstactObjectAnimations 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Анимация персонажа")]
        protected Animation animationOfPlayer;
        [SerializeField, Tooltip("Лист векторов")]
        protected List<Vector2> animationsStateList = new List<Vector2>();
        protected string animationName;
        protected float frameLenght;

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
