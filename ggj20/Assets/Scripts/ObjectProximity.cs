using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class ObjectProximity : MonoBehaviour
    {
        [HideInInspector] public ObjectEnum objectEnum;

        private void OnTriggerStay(Collider other)
        {
            var distance = Vector3.Distance(other.transform.position, transform.position);
            Debug.Log($"{objectEnum} {distance}");
        }
    }
}