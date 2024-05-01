namespace ProjectZ.Core.Characters
{
    public interface IDamagable
    {
        public void TakeDamage(int damageAmount);
        public void OnUpdateHP(int value);
        public void OnDeath();
    }
}