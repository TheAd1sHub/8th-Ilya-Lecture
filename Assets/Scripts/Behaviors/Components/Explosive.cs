using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheMinefield.Behaviors
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Explosive : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private bool _playExplosionEffect = false;
        [SerializeField] private ParticleSystem _explosionEffectPrefab;
        [Space]
        [SerializeField] private bool _destroyOnExplosion = true;
        [field: SerializeField, Min(0)] public int Damage { get; set; } = 10;
        [field: SerializeField, Min(0)] private float ExplosionRadius { get; set; } = 3f;
        [field: SerializeField, Min(0)] private float ExplosionDelaySeconds { get; set; } = 1.5f;

        public ExplosionState State { get; private set; } = ExplosionState.NotActivated;

        private string RepeatedActivationErrorMessage => $"'{name}': Cannot Explode: The Bomb was Already Activated";

        public void Explode()
        {
            if (State != ExplosionState.NotActivated)
                throw new InvalidOperationException(RepeatedActivationErrorMessage);

            StartCoroutine(ExplodeOnTimeout(ExplosionDelaySeconds));
        }

        public void ExplodeImmediate()
        {
            if (State == ExplosionState.Exploded)
                throw new InvalidOperationException(RepeatedActivationErrorMessage);

            IEnumerable<ExplosionAffected> targets = Physics
                .SphereCastAll(transform.position, ExplosionRadius, Vector3.up)
                .Select(hitInfo => hitInfo.collider.GetComponent<ExplosionAffected>())
                .Where(@object => @object != null);

            if (_playExplosionEffect)
            {
                ParticleSystem explosionEffect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
                explosionEffect.Play();
            }

            foreach (ExplosionAffected target in targets)
                target.Damageable.ApplyDamage(Damage);

            State = ExplosionState.Exploded;

            if (_destroyOnExplosion)
                Destroy(gameObject);
        }

        private IEnumerator ExplodeOnTimeout(float timeoutSeconds)
        {
            State = ExplosionState.Activated;

            yield return new WaitForSeconds(timeoutSeconds);
            ExplodeImmediate();
        }

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        public enum ExplosionState { NotActivated, Activated, Exploded }
    }
}
