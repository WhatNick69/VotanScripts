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
        /// <summary>
        /// Инициализация генерации скорости облаков
        /// </summary>
        private void Start()
        {
            RandomSetSpeedAnimation();
        }

        /// <summary>
        /// Сгенерить скорость облаков
        /// </summary>
        private void RandomSetSpeedAnimation()
        {
            foreach (Transform child in transform)
            {
                foreach (Transform childOfChild in child)
                {
                    childOfChild.GetComponent<Animation>()["CloudAnimation"].speed
                    = LibraryStaticFunctions.GetRangeValue(0.0075f, 0.25f);
                }
            }
        }
    }
}
