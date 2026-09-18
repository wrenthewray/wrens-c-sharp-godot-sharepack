using Godot;

namespace Shared.Components.Follow;

[GlobalClass, Icon("res://addons/at-icons/node3d/ghost.svg")]
public partial class Follow3DComponent : Node
{
    protected Node3D parent;
    [Export] public Node3D target;
    [Export] protected Vector3 offset;
    [Export] protected FollowType followType = FollowType.Smooth;
    [Export] protected float followSpeed = 4.0f;

    public override void _Ready()
    {
        base._Ready();

        parent = GetParent<Node3D>();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    
        switch(followType)
        {
            case FollowType.Simple:
                parent.Position = target.Position + offset;
            break;
            case FollowType.Smooth:
                float weight = 1f - Mathf.Exp(-followSpeed * (float)delta);
                parent.Position = parent.Position.Lerp(target.Position + offset, weight);
            break;
        }
    }
}
public enum FollowType
{
    Simple,
    Smooth,
    
}