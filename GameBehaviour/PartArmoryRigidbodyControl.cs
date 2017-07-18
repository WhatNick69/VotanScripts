using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace GameBehaviour
{
    public class PartArmoryRigidbodyControl
        : MonoBehaviour
    {
        private Rigidbody rigFromObj;

        private void Start()
        {
            rigFromObj = GetComponent<Rigidbody>();
            rigFromObj.detectCollisions = false;
        }

        public void FireEvent()
        {
            DetachFromParent();
            AddForceToObject();
            Timing.RunCoroutine(CoroutineForDisableObject());
        }

        private void DetachFromParent()
        {
            transform.parent = null;
        }

        private void AddForceToObject()
        {
            rigFromObj.useGravity = true;
            rigFromObj.detectCollisions = true;
            rigFromObj.isKinematic = false;
            rigFromObj.constraints = RigidbodyConstraints.None;
            rigFromObj.AddForce(new Vector3(LibraryStaticFunctions.GetPlusMinusValue(75),
                LibraryStaticFunctions.rnd.Next(40, 100),
                LibraryStaticFunctions.GetPlusMinusValue(75)));
        }

        private IEnumerator<float> CoroutineForDisableObject()
        {
            yield return Timing.WaitForSeconds(0.3f);
            GetComponent<BoxCollider>().enabled = true;
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.rnd.Next(5, 10));
            gameObject.SetActive(false);
        }
    }
}
