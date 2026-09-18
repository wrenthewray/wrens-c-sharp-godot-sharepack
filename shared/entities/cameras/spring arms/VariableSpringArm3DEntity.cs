using Godot;

namespace Shared.Entities.Cameras.SpringArms;

[GlobalClass]
public partial class VariableSpringArm3DEntity : SpringArm3D
{
    public void SetSpringLength(float length)
    {
        SpringLength = length;
    }
}