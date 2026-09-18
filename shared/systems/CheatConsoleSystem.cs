
using Godot;

namespace Shared.Systems;

[GlobalClass]
public partial class CheatConsoleSystem : Node
{
    private static Vector2 GetRandomDirection()
    {
        return new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized();
    }
    public string ListTreeNodes()
    {
        string allNodes = "";
        foreach (Node node in GetTree().CurrentScene.GetChildren())
        {
            allNodes += AddChildrenRecursive(node, 0);
        }
        return allNodes;
    }
    private string AddChildrenRecursive(Node currentNode, int layer)
    {
        string nodePath = "";
        for (int i = 0; i < layer; i++)
            nodePath += "\t";

        nodePath += currentNode.Name + "\n";
        if (currentNode.GetChildren().Count > 0)
            foreach (Node child in currentNode.GetChildren())
                nodePath += AddChildrenRecursive(child, layer + 1);
            
        return nodePath;
    }
}