using Godot;

namespace Shared.Components.Stats;

/// <summary>
/// A base class for all stats used for entities. Defines a basic structure that 
/// other stats follow.
/// </summary>
[GlobalClass]
public abstract partial class StatComponent : Node
{
    [Export] public int maxValue;
    public float currentValue;

    public override void _Ready()
    {
        ResetValue();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        CapValue();
    }
    /// <summary>
    /// A function called in <see cref="Node._Process(double)"/> that
    /// caps the <see cref="currentValue"/> to between 0 and <see cref="maxValue"/>.
    /// </summary>
    internal virtual void CapValue()
    {
        if (currentValue < 0.01f) currentValue = 0;
        if (currentValue > maxValue) currentValue = maxValue;
    }
    public virtual void ResetValue()
    {
        currentValue = maxValue;
    }
    public bool IsEmpty()
    {
        return Mathf.IsZeroApprox(currentValue);
    }
    public bool IsFull()
    {
        return Mathf.IsEqualApprox(currentValue, maxValue);
    }
}
