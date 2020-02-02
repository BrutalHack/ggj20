using System;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ObjectProximity : MonoBehaviour
    {
        [HideInInspector] public ObjectEnum objectEnum;
        private float _maxDistance;

        private void Awake()
        {
            _maxDistance = GetComponent<CircleCollider2D>().radius;
        }

        private void OnTriggerStay(Collider other)
        {
            var distance = Vector3.Distance(other.transform.position, transform.position);
            Debug.Log($"{objectEnum} {distance} {_maxDistance}");
        }
    }
}