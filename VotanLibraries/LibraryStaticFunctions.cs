using UnityEngine;

namespace VotanLibraries
{
    /// <summary>
    /// Статические функции
    /// </summary>
    public class LibraryStaticFunctions
        : MonoBehaviour
    {
        public static System.Random rnd = new System.Random();

        /// <summary>
        /// В указанном диапазоне возвращает значение
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static float GetRangeValue(float dmg, float range=0)
        {
            if (range > 1) range = 1;
            else if (range < 0) range = 0;

            return dmg + (float)((rnd.NextDouble()
                * 2 - 1) * dmg * range); 
        }

        /// <summary>
        /// Вернуть значение + либо -
        /// </summary>
        /// <param name="valueMax"></param>
        /// <returns></returns>
        public static float GetPlusMinusValue(float valueMax)
        {
            return (float)rnd.NextDouble() * valueMax * 2 - valueMax;
        }

        /// <summary>
        /// Случайная позиция врага во время отдыха
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float GetRandomAxisOfEnemyRest(float radius)
        {
            return (float)(rnd.NextDouble()* (radius*2))-radius;
        }
    }
}
