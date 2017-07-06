using UnityEngine;

namespace PlayerBehaviour
{
    public class Grip 
        : MonoBehaviour
    {
        [SerializeField, Range(1, 100)]
        private float weight;
        [SerializeField, Range(0, 3)]
        private int gripType;
        [SerializeField]
        private string weaponName;
        [SerializeField, Range(0, 30)]
        private float bonusSpinSpeed;

        public float GetBonusSpinSpeed()
        {
            return bonusSpinSpeed;
        }

        public float GetWeight()
        {
            return weight;
        }

        public int GetGripType()
        {
            return gripType;
        }

        public string GetWeaponName()
        {
            return weaponName;
        }
    }
}
