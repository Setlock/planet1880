using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    protected string name;
    public Node()
    {

    }
    public Node(string name)
    {
        this.name = name;
    }
    public virtual NodeStatus Tick(float deltaTime)
    {
        return NodeStatus.Success;
    }
}
