using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    Node root;
    public BehaviorTree(){}
    public BehaviorTree(Node root)
    {
        this.root = root;
    }
    public void Tick(float deltaTime)
    {
        root.Tick(deltaTime);
    }
    public void SetRoot(Node node)
    {
        this.root = node;
    }
}
