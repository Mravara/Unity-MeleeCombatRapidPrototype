using System;
using UnityEngine;

namespace Binx
{
    public class ColliderLink : MonoBehaviour
    {
        public Action<Collision> OnCollisionEnterEvent;
        public Action<Collision> OnCollisionExitEvent;
        public Action<Collider> OnTriggerEnterEvent;
        public Action<Collider> OnTriggerExitEvent;
        
        [SerializeField] private bool triggerEnter;
        [SerializeField] private bool triggerExit;
        [SerializeField] private bool collisionEnter;
        [SerializeField] private bool collisionExit;

        private void OnCollisionEnter(Collision other)
        {
            if (collisionEnter)
            {
                OnCollisionEnterEvent?.Invoke(other);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (collisionExit)
            {
                OnCollisionExitEvent?.Invoke(other);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerEnter)
            {
                OnTriggerEnterEvent?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerExit)
            {
                OnTriggerExitEvent?.Invoke(other);
            }
        }
    }
}
