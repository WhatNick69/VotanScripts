using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Анимации внутриигровых объектов. Вызов их событий 
    /// (ящики, бочки и прочее).
    /// </summary>
    public class GameAnimationsObjects
        : MonoBehaviour
    {
        private static GameObject[] listStaticDynamicObjects;
        private static bool[] boolListStaticDynamicObjects;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            listStaticDynamicObjects = 
                GameObject.FindGameObjectsWithTag("DynamicGameobject");
            boolListStaticDynamicObjects = new bool[listStaticDynamicObjects.Length];
        }

        /// <summary>
        /// Зажечь событие у игрового объекта
        /// </summary>
        /// <param name="numberOfObject"></param>
        public static void FireEventForDynamicObject(int numberOfObject)
        {
            if (boolListStaticDynamicObjects.Length != 0)
            {
                if (!boolListStaticDynamicObjects[numberOfObject])
                {
                    listStaticDynamicObjects[numberOfObject].
                        GetComponent<Animation>().Play();
                    boolListStaticDynamicObjects[numberOfObject] = true;
                }
            }
        }
    }
}
