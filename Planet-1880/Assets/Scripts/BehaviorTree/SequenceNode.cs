using System.Collections;
using System.Collections.Generic;

public class SequenceNode : Node
{
    List<Node> children = new List<Node>();

    public SequenceNode(string name)
    {
        this.name = name;
    }
    public override NodeStatus Tick(float deltaTime)
    {
        foreach(Node behaviorNode in children)
        {
            NodeStatus childStatus = behaviorNode.Tick(deltaTime);
            if(childStatus != NodeStatus.Success)
            {
                return childStatus;
            }
        }
        return NodeStatus.Success;
    }
    public void AddChild(Node child)
    {
        children.Add(child);
    }
}
