using UnityEngine;

namespace VotanLibraries
{
    /// <summary>
    /// Библиотека собственной реализации физики
    /// </summary>
    public class LibraryPhysics
        : MonoBehaviour
    {
        /// <summary>
        /// Первые две переменные задают положение оружия(отрезок), 
        /// их можно вводить в любой последовательности 
        /// Следующие две переменные задают положение врага(отрезок), 
        /// их так же можно вводить в любой последовательности
        /// Тк идет рассчет проекций на плоскость, то положение объектов по Y никак не влияет на разультат
        /// На выхоже мы получаем true если отрезки пересеклись, в противном случае получаем false
        /// Что бы не было проблем с рассчетом столкновений 2х параллельных отрезков, 
        /// лучше вызвать функцию еще раз, 
        /// но уже сдругими точками задающими положение врага
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static bool Vector3Crossing(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            float a = (x.x - y.x) * (w.z - z.z) - (x.z - y.z) * (w.x - z.x);
            float b = (x.x - z.x) * (w.z - z.z) - (x.z - z.z) * (w.x - z.x);
            float c = (x.x - y.x) * (x.z - z.z) - (x.z - y.z) * (x.x - z.x);

            float ta = b / a;
            float tb = c / a;

            return (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1);
        }

        /// <summary>
        /// Проверка пересечения двух векторов
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static bool BushInLine(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            float a = (x.x - y.x) * (w.z - z.z) - (x.z - y.z) * (w.x - z.x);
            float b = (x.x - z.x) * (w.z - z.z) - (x.z - z.z) * (w.x - z.x);
            float c = (x.x - y.x) * (x.z - z.z) - (x.z - y.z) * (x.x - z.x);

            float ta = b / a;
            float tb = c / a;

            return (ta >= 0 && ta <= 1.6 && tb >= 0 && tb <= 1.6);
        }

        /// <summary>
        /// Принадлежность точки плоскости
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static bool BushInPlane(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            float a = (y.x - x.x) * (z.z - y.z) - (z.x - y.x) * (y.z - x.z);
            float b = (z.x - x.x) * (w.z - z.z) - (w.x - z.x) * (z.z - x.z);
            float c = (w.x - x.x) * (y.z - w.z) - (y.x - w.x) * (w.z - x.z);

            return ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0));
        }

        /// <summary>
        /// Перегруженный метод
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <param name="AttackPoint"></param>
        /// <returns></returns>
        public static Vector3 Vector3Crossing(Vector3 x, Vector3 y, Vector3 z, Vector3 w, bool AttackPoint)
        {
            float a = (x.x - y.x) * (w.z - z.z) - (x.z - y.z) * (w.x - z.x);
            float b = (x.x - z.x) * (w.z - z.z) - (x.z - z.z) * (w.x - z.x);
            float c = (x.x - y.x) * (x.z - z.z) - (x.z - y.z) * (x.x - z.x);

            float ta = b / a;
            float tb = c / a;

            if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
            {
                float A = x.x + ta * (y.x - x.x);
                float B = x.z + ta * (y.z - x.z);

                return new Vector3(A, 2, B);
            }
            else return Vector3.zero;
        }
    }
}
