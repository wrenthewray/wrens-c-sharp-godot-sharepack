using Godot;

namespace Shared.Components.Stats
{
    /// <summary>
    /// This class is a base class for any stat that
    /// regenerates over time.
    /// </summary>
    [GlobalClass]
    public abstract partial class RegenStatComponent : StatComponent
    {
        internal Timer regenTimer;
        [Export] internal float regenRate;
        [Export] internal float regenWaitTime;

        internal bool regen = false;
        internal float regenModifier = 1;
        public override void _Ready()
        {
            base._Ready();
            regenTimer = new();
            AddChild(regenTimer);
            regenTimer.Timeout += AllowRegen;
            regenTimer.WaitTime = regenWaitTime;
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            Regen(delta);
        }

        internal override void CapValue()
        {
            if(currentValue < 0.01f) currentValue = 0;
            if(currentValue > maxValue) 
            {
                currentValue = maxValue;
                regen = false;
            }
        }

        internal virtual void AllowRegen() => regen = true;
        /// <summary>
        /// Called in the <see cref="Node._Process(double)"/> 
        /// method, this method controls the regeneration 
        /// of the stat.
        /// </summary>
        internal virtual void Regen(double delta)
        {
            if(!regen) 
                return;
            currentValue += regenModifier * regenRate * (float) delta * (float) (1 + (1 - Engine.TimeScale));
        }
    }
    
}