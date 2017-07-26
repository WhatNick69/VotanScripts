using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace VotanGameplay
{
    /// <summary>
    /// Контролирует скорость облаков
    /// </summary>
    public class CloudObjectsController 
        : MonoBehaviour
    {
        [SerializeField,Tooltip("Лист облаков-объектов")]
        private List<GameObject> clouds;

        /// <summary>
        /// Инициализация генерации скорости облаков
        /// </summary>
        private void Start()
        {
            RandomSetspeedAnimation();
        }

        /// <summary>
        /// Сгенерить скорость облаков
        /// </summary>
        private void RandomSetspeedAnimation()
        {
            foreach (GameObject cloud in clouds)
                cloud.GetComponent<Animation>()["CloudAnimation"].speed
                    = LibraryStaticFunctions.GetRangeValue(0.0075f, 0.25f);
        }
    }
}
