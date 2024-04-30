namespace ProjectZ.Core.FSM
{
    public interface IState
    {
        /// <summary>
        /// 상태 진입
        /// </summary>
        public void OperateEnter();
        /// <summary>
        /// 상태 진행 중
        /// </summary>
        public void OperateUpdate();
        /// <summary>
        /// 상태 나감
        /// </summary>
        public void OperateExit();
    }
}
