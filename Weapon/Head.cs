using UnityEngine;

namespace PlayerBehaviour
{
    public class Head 
        : MonoBehaviour
    {
        [SerializeField, Range(1, 100)]
        private float weight;
        [SerializeField, Range(0, 1)] // 0 рубящая, 1 дробящая
        private int headType;
        [SerializeField]
        private string weaponName;
        [SerializeField, Range(0, 30)]
        float bonusSpinSpeed;

        public float GetBonusSpinSpeed()
        {
            return bonusSpinSpeed;
        }

        public float GetWeight()
        {
            return weight;
        }

        public int GetAttackType()
        {
            return headType;
        }

        public string GetName()
        {
            return name;
        }
    }
}
