using Godot;

namespace Shared.Components.Position;

[GlobalClass, Icon("res://addons/at-icons/node3d/footsteps.svg")]
public partial class Position3DComponent : Node
{
    protected Node3D parent;
    [Export] protected float moveSpeed = 15f;

    private Vector2 direction = Vector2.Zero;
    public override void _Ready()
    {
        base._Ready();

        parent = GetParent<Node3D>();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        parent.Position += new Vector3(
            direction.X * (float) delta * moveSpeed,
            0f,
            direction.Y * (float) delta * moveSpeed
        ).Rotated(Vector3.Up, parent.Rotation.Y);
    }
    public void ChangeDirection(float Y, float X)
    {
        direction = new Vector2(X, Y);
    }
}