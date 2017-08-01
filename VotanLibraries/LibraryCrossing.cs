using UnityEngine;

namespace VotanLibraries
{
    public class LibraryCrossing
        : MonoBehaviour
    {

        [SerializeField]
        Transform l1, l2, p1, p2, p3, p4;
        bool isIntersection;

        void Drawer()
        {
            if (isIntersection)
                Debug.DrawLine(l1.position, l2.position, Color.green, 0.1f);
            else
                Debug.DrawLine(l1.position, l2.position, Color.red, 0.1f);

            Debug.DrawLine(p1.position, p2.position, Color.blue, 0.1f);
            Debug.DrawLine(p2.position, p3.position, Color.blue, 0.1f);
            Debug.DrawLine(p3.position, p4.position, Color.blue, 0.1f);
            Debug.DrawLine(p4.position, p1.position, Color.blue, 0.1f);
            Debug.DrawLine(p4.position, p2.position, Color.blue, 0.1f);
            Debug.DrawLine(p1.position, p3.position, Color.blue, 0.1f);
        }
    }
}
