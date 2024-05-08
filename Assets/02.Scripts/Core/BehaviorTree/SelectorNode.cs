using System.Collections.Generic;

namespace ProjectZ.Core.BehaviorTree
{
    /// <summary>
    /// 왼쪽에서 오른쪽으로 판단을 진행하면서 자식 노드가 성공이나 진행 중을 반환하면 부모 노드에 즉시 결과를 반환
    /// </summary>
    public class SelectorNode : INode
    {
        private List<INode> _childNodeList;

        public SelectorNode(List<INode> childNodeList)
        {
            _childNodeList = childNodeList;
        }

        public INode.ENodeState Evaluate()
        {
            if (_childNodeList == null || _childNodeList.Count == 0)
                return INode.ENodeState.FailureState;

            for (int index = 0; index < _childNodeList.Count; index++)
            {
                switch (_childNodeList[index].Evaluate())
                {
                    case INode.ENodeState.RunningState:
                        return INode.ENodeState.RunningState;
                    case INode.ENodeState.SuccessState:
                        return INode.ENodeState.SuccessState;
                }
            }

            return INode.ENodeState.FailureState;
        }
    }
}
