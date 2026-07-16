
using Godot;

namespace Shared.Models
{
    /// <summary>
    /// This class describes the information passed to other components
    /// on a successful attack.
    /// </summary>
    [GlobalClass, Icon("res://addons/at-icons/node/dagger.svg")]
    public partial class Attack : Resource
    {
        /// <summary>
        /// Damage done to the <see cref="Components.Stats.Health.HealthComponent"/>.
        /// </summary>
        [Export] public int damage;
        [Export] public int knockbackForce;
        // /// <summary>
        // /// Damage done to the <see cref="Components.Stats.Poise.PoiseComponent"/>.
        // /// </summary>
        // public int stanceDamage;
        /// <summary>
        /// Modifier that's multiplied by the total damage.
        /// </summary>
        public float damageModifier = 1;
        /// <summary>
        /// The direction of knockback, determined by the attacker and attackee.
        /// </summary>
        public Vector3 knockbackDirection;
    }
}