using Godot;
using Shared.Resources;

namespace Shared.Components.Hitbox;

/// <summary>
/// This class acts as the hitbox for all entities. If an entity 
/// can take damage, they must do so through this component. 
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/receiver.svg")]
public partial class HitboxComponent3D : Area3D
{
    [Signal] public delegate void DamagedEventHandler(Attack attack);
    [Export] private bool invulnerable;
    public override void _Ready()
    {
        base._Ready();
    }

    public void Damage(Attack attack)
    {
        EmitSignal(SignalName.Damaged, attack);
    }
    public void SetInvulnerable(bool invulnerable)
    {
        this.invulnerable = invulnerable;
    }

}