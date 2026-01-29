using System.Collections.Generic;

//Node Status
public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}


public abstract class Node
{
    protected NodeState state;
    public Node parent;
    protected List<Node> children = new List<Node>();

    public Node()
    {
    }

    public Node(List<Node> children)
    {
        foreach (var child in children)
        {
            Attach(child);
        }
    }

    public void Attach(Node child)
    {
        child.parent = this;
        this.children.Add(child);
    }

    public abstract NodeState Evaluate();

    public T FindNode<T>() where T : Node
    {
        if (this is T foundNode) return foundNode;
        foreach (Node child in children)
        {
            T result = child.FindNode<T>();
            if (result != null) return result;
        }
        return null;
    }
}