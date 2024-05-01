namespace ProjectZ.Data
{
    public class AttackData
    {
        /// <summary>
        /// 실행할 애니 해시
        /// </summary>
        public int AttackAnimHash;
        /// <summary>
        /// 후속 공격 시 전환 지점
        /// </summary>
        public float TransitionNormalizedTime;

        public AttackData(int attackAnimHash, float transitionNormalizedTime)
        {
            AttackAnimHash = attackAnimHash;
            TransitionNormalizedTime = transitionNormalizedTime;
        }
    }
}
