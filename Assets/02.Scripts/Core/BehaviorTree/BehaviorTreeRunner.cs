namespace ProjectZ.Core.BehaviorTree
{
    public class BehaviorTreeRunner
    {
        private INode _rootNode;

        public BehaviorTreeRunner(INode rootNode)
        {
            _rootNode = rootNode;
        }

        public void Operate()
        {
            _rootNode.Evaluate();
        }
    }
}
