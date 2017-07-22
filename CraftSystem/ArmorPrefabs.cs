using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftSystem
{
    public class ArmorPrefabs : MonoBehaviour
    {

        GameObject cuirass;
        GameObject helmet;
        GameObject shield;


        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public GameObject Cuirass
        {
            get { return cuirass; }
            set { cuirass = value; }
        }

        public GameObject Helmet
        {
            get { return helmet; }
            set { helmet = value; }
        }

        public GameObject Shield
        {
            get { return shield; }
            set { shield = value; }
        }
    }
}
