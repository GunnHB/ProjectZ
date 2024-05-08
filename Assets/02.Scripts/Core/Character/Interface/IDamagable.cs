namespace ProjectZ.Core.Characters
{
    public interface IDamagable
    {
        public void TakeDamage(int damageAmount);
        public void PlayGetDamageAnimation(int value);
        public void PlayDeathAnimation();
    }
}