using System;

namespace ProjectZ.Core.BehaviorTree
{
    public class ActionNode : INode
    {
        private Func<INode.ENodeState> OnUpdateFunc;

        public ActionNode(Func<INode.ENodeState> updateFunc)
        {
            OnUpdateFunc = updateFunc;
        }

        // OnUpdateFunc에 이벤트가 달렸으면 실행
        // 없으면 failure 반환
        public INode.ENodeState Evaluate() => OnUpdateFunc?.Invoke() ?? INode.ENodeState.FailureState;
    }
}