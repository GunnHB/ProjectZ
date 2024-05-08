namespace ProjectZ.Core.BehaviorTree
{
    public interface INode
    {
        public enum ENodeState
        {
            None = -1,
            FailureState,   // 실패
            RunningState,   // 진행 중
            SuccessState,   // 성공
        }

        public ENodeState Evaluate();
    }
}
