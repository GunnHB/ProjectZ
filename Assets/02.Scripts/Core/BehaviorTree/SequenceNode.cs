using System.Collections.Generic;

namespace ProjectZ.Core.BehaviorTree
{
    /// <summary>
    /// 왼쪽에서 오른쪽으로 판단을 진행하면서 자식 노드 중 하나라도 실패를 반환하면 바로 부모 노드에 실패를 반환
    /// <para>모든 자식 노드가 성공을 반환해야 부모 노드에 성공을 반환</para>
    /// </summary>
    public class SequenceNode : INode
    {
        private List<INode> _childNodeList;

        public SequenceNode(List<INode> childNodeList)
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
                    case INode.ENodeState.FailureState:
                        return INode.ENodeState.FailureState;
                    case INode.ENodeState.RunningState:
                        return INode.ENodeState.RunningState;
                    case INode.ENodeState.SuccessState:
                        continue;
                }
            }

            return INode.ENodeState.SuccessState;
        }
    }
}
