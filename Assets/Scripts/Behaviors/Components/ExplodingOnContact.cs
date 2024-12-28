using TheMinefield.Utils.ConditionalLogging;
using UnityEngine;

namespace TheMinefield.Behaviors
{
    [RequireComponent(typeof(Explosive), typeof(SphereCollider))]
    public sealed class ExplodingOnContact : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _triggerRadius;
        [SerializeField] private SphereCollider _triggerCollider;
        [Space]
        [SerializeField] private bool _drawExplosionGizmos = true;

        private Explosive _explosive;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<TriggerringExplosions>(out _))
                _explosive.Explode();
        }

        private void OnValidate()
        {
            _triggerCollider ??= GetComponent<SphereCollider>();
            _triggerCollider.radius = _triggerRadius;
        }

        private void OnDrawGizmos()
        {
            if (_drawExplosionGizmos)
                Gizmos.DrawSphere(transform.position, _triggerRadius);

            ConditionalLogger.WarnIf.HasNoTrigger(this);
        }

        private void Awake()
        {
            _explosive = GetComponent<Explosive>();
        }
    }
}
