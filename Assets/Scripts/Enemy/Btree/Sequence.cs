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
        bool isAnyChildRunning = false;
        foreach (var node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    state = NodeState.FAILURE;
                    return state;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    isAnyChildRunning = true;
                    break;
            }
        }
        state = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return state;

    }
}