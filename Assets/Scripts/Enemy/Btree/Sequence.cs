using System.Collections.Generic;

public class Sequence : Node
{
    public Sequence()
    {
    }

    public Sequence(List<Node> children) : base(children)
    {
    }

    public override NodeState Evaluate()
    {
        foreach (var node in children)
        {
            var result = node.Evaluate();
            if (result == NodeState.FAILURE)
                return state = NodeState.FAILURE;
            if (result == NodeState.RUNNING)
                return state = NodeState.RUNNING;          
        }
        return state = NodeState.SUCCESS;
    }
}