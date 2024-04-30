using UnityEngine;
using UnityEngine.Events;

namespace ProjectZ.Item
{
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponTester : MonoBehaviour
    {
        [SerializeField] private Collider _hitCollider;
        public Collider HitCollider => _hitCollider;

        private void Awake()
        {
            _hitCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != Manager.GameValue.LAYER_ENEMY)
                return;

            if (other.TryGetComponent(out Core.Characters.IDamagable damagable))
            {
                damagable.TakeDamage(10);
            }
        }
    }
}
