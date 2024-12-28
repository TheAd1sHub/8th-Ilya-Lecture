using UnityEngine;

namespace TheMinefield.Behaviors
{
    [RequireComponent(typeof(Damageable))]
    public sealed class ExplosionAffected : MonoBehaviour
    {
        public Damageable Damageable { get; private set; }

        private void Awake()
        {
            Damageable = GetComponent<Damageable>();
        }
    }
}
