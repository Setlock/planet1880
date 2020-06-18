using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    List<Node> children = new List<Node>();
    public SelectorNode(string name)
    {
        this.name = name;
    }
    public override NodeStatus Tick(float deltaTime)
    {
        foreach(Node child in children)
        {
            NodeStatus childStatus = child.Tick(deltaTime);
            if(childStatus != NodeStatus.Failure)
            {
                return childStatus;
            }
        }
        return NodeStatus.Failure;
    }
    public void AddChild(Node child)
    {
        children.Add(child);
    }
}
