using Godot;
using Shared.Models;

namespace Shared.Components.Stats.Health
{
    /// <summary>
    /// This is the main health component for all entities. We attach this component 
    /// to any entity that takes damage in any sort of way.
    /// <br/><br/>
    /// This component requires the <see cref="Hitbox.HitboxComponent"/>
    /// to send an <see cref="Attack"/> parameter to its <see cref="Damage"/> 
    /// </summary>
    [GlobalClass, Icon("res://addons/at-icons/node/heart.svg")]
    public partial class HealthComponent : StatComponent
    {
        [Signal] public delegate void DeathEventHandler();

        public float damageMultiplier = 1;
        /// <summary>
        /// This is called by an attached <see cref="Hitbox.HitboxComponent"/>
        /// to deal damage to an entity's health.
        /// </summary>
        /// <param name="attack">Information on the attack dealt to the entity. 
        /// This class uses the <see cref="Attack.damage"/> variable to  </param>
        public void Damage(Attack attack)
        {
            currentValue -= Mathf.Round(attack.damage * attack.damageModifier * damageMultiplier);

            if (IsEmpty())
                EmitSignal(SignalName.Death);
        }
        
    }
    
}