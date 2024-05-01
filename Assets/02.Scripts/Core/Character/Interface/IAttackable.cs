namespace ProjectZ.Core.Characters
{
    public interface IAttackable
    {
        public void InitAttackDatas();
        public void AttackTarget(CharacterControls controls);
    }
}
