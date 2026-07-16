using Godot;
using System;

namespace Shared.Components.Stats.Mana
{
    /// <summary>
    /// This component is only used by the player entity. It
    /// controls the player's mana, which all actions the player
    /// takes use. If this stat is fully emptied - that is, 
    /// depleted to zero - then the player will not be able to
    /// use an action for the duration of 
    /// </summary>
    [GlobalClass, Icon("res://components/stats/mana/icon_star.png")]
    public partial class ManaComponent : RegenStatComponent
    {
        [Signal] public delegate void ManaEmptiedEventHandler();
        [Export] private float depletedRegenModifier = 0.75f;
        public bool emptied;
        
        public void Deplete(float amount)
        {
            regenTimer.Stop();

            regen = false;
            currentValue -= amount;

            regenTimer.Start(0);
        }
        public void Restore(float amount)
        {
            regenTimer.Stop();

            regen = false;
            currentValue += amount;
            
            regenTimer.Start(0);
        }

        internal override void CapValue()
        {
            if(currentValue < 0.01f)
            {
                currentValue = 0;
                emptied = true;
                EmitSignal(SignalName.ManaEmptied);
                regenModifier = depletedRegenModifier;
            }
            if(currentValue > maxValue) 
            {
                currentValue = maxValue;
                regen = false;
                emptied = false;
                regenModifier = 1;
            }
        }
    }
}
