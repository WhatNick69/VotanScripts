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
        public static float GetPlusMinusVal(float dmg, float range=0)
        {
            if (range > 1) range = 1;
            else if (range < 0) range = 0;

            return dmg + (float)((rnd.NextDouble()
                * 2 - 1) * dmg * range); 
        }
    }
}
