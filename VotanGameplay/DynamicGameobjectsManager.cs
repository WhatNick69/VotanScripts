using UnityEngine;
using VotanInterfaces;

namespace VotanGameplay
{
    /// <summary>
    /// Анимации внутриигровых объектов. Вызов их событий 
    /// (ящики, бочки и прочее).
    /// </summary>
    public class DynamicGameobjectsManager
        : MonoBehaviour
    {
        private static IDynamicGameobject[] listDynamicObjectsInterfaces;
        private static bool[] listDynamicBool;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            GameObject[] listDynamicObjects = 
                GameObject.FindGameObjectsWithTag("DynamicGameobject");
            listDynamicObjectsInterfaces = 
                new IDynamicGameobject[listDynamicObjects.Length];

            for (int i = 0; i < listDynamicObjects.Length; i++)
                listDynamicObjectsInterfaces[i] = 
                    listDynamicObjects[i].GetComponent<IDynamicGameobject>();
            listDynamicBool = new bool[listDynamicObjectsInterfaces.Length];
        }

        /// <summary>
        /// Зажечь событие у игрового объекта
        /// </summary>
        /// <param name="numberOfObject"></param>
        public static void FireEventDynamicObject(int numberOfObject)
        {
            if (listDynamicBool.Length != 0)
            {
                if (!listDynamicBool[numberOfObject])
                {
                    listDynamicObjectsInterfaces[numberOfObject].FireEvent();
                    listDynamicBool[numberOfObject] = true;
                }
            }
        }

        /// <summary>
        /// Рестарт объекта
        /// </summary>
        /// <param name="numberOfObject"></param>
        public static void RestartDynamicObject(int numberOfObject)
        {
            listDynamicObjectsInterfaces[numberOfObject].RestartObject();
            listDynamicBool[numberOfObject] = false;
        }
    }
}
