using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Поворачивает интерфейс под объектом
    /// </summary>
    public class TriaglesRender
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
        private Transform A, B, C, A1, B1, C1;
        [SerializeField]
        private Transform stairs;
        private Vector3 positionOfStairs;
		private Transform center;
		private float AB, AC, BC, AA1;

		private float normalLevel;
        private float a, b, c, d, e, f;
        private float S;
        private float H;
        private float sootn;
        private static float Y;
		private bool gravity = true;
		private bool onLevelOne = true;
		private bool onLevelTwo = false;
        [SerializeField,Tooltip("Угол")]
        private float angle;

        private static List<TriaglesRender> trianglesOnTheScene;
        #endregion

        private void Awake()
        {
            trianglesOnTheScene = new List<TriaglesRender>();

            AB = Vector3.Distance(A.position, B.position);
            AC = Vector3.Distance(A.position, C.position);
            BC = Vector3.Distance(C.position, B.position);
            AA1 = Vector3.Distance(A.position, A1.position);
            center = transform;
            trianglesOnTheScene.Add(this);
            normalLevel = 0.55f;
            positionOfStairs = stairs.position;
        }

        /// <summary>
        /// Находимся ли мы на лестнице
        /// </summary>
        /// <param name="positionObject"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsOnTheStairs(Vector3 positionObject, ref float value)
        {
            bool flag = false;
            foreach (TriaglesRender stairs in trianglesOnTheScene)
            {
                if (Vector3.Distance(positionObject,stairs.positionOfStairs) <= 2f)
                    flag = stairs.HightOnY(positionObject, ref value);
            }
            return flag;
        }

        /// <summary>
        /// Проверяет принадлежность точки "X" к плоскости заданной точками y,x,w
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private bool InPlane(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            Vector3 playerPoint = x;
            Vector3 PV1 = y;
            Vector3 PV2 = z;
            Vector3 PV3 = w;

            a = (PV1.x - playerPoint.x) * (PV2.z - PV1.z) - (PV2.x - PV1.x) * (PV1.z - playerPoint.z);
            b = (PV2.x - playerPoint.x) * (PV3.z - PV2.z) - (PV3.x - PV2.x) * (PV2.z - playerPoint.z);
            c = (PV3.x - playerPoint.x) * (PV1.z - PV3.z) - (PV1.x - PV3.x) * (PV3.z - playerPoint.z);

            return ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0));
        }

        /// <summary>
        /// Расчитывает нужную высоту позиции персонажа по Y
		/// с помощью подобия треугольников
        /// </summary>
        private bool HightOnY(Vector3 position, ref float value)
        {
            if (InPlane(position, B.position, A1.position, A.position) ||
                InPlane(position, B1.position, B.position, A1.position))
            {
                value = angle;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
