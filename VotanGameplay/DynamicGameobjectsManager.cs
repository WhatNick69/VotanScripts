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
        private static Transform[] listDynamicObjectsTransform;
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
            listDynamicObjectsTransform =
                new Transform[listDynamicObjects.Length];

            for (int i = 0; i < listDynamicObjects.Length; i++)
            {
                listDynamicObjectsInterfaces[i] =
                    listDynamicObjects[i].GetComponent<IDynamicGameobject>();
                listDynamicObjectsTransform[i] =
                    listDynamicObjects[i].transform;
            }
            listDynamicBool = new bool[listDynamicObjectsInterfaces.Length];
        }

        /// <summary>
        /// Получить динамический объект по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetObjectTransform(string name)
        {
            for (int i = 0; i < listDynamicObjectsTransform.Length; i++)
            {
                if (listDynamicObjectsTransform[i] != null
                    && listDynamicObjectsTransform[i].name.Equals(name))
                {
                    return listDynamicObjectsTransform[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Зажечь событие у игрового объекта
        /// </summary>
        /// <param name="numberOfObject"></param>
        public static void FireEventDynamicObject(string name)
        {
            if (listDynamicBool.Length != 0)
            {
                int i = 0;
                bool equals = false;

                // проверяем на сходство по имени
                for (i = 0; i < listDynamicObjectsTransform.Length; i++)
                {
                    if (listDynamicObjectsTransform[i] != null
                        && listDynamicObjectsTransform[i].name.Equals(name))
                    {
                        equals = true;
                        break;
                    }
                }
                if (!equals) return;

                // запускаем объект
                if (!listDynamicBool[i])
                {
                    listDynamicObjectsInterfaces[i].FireEvent();
                    listDynamicBool[i] = true;
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
