using UnityEngine;

namespace ProjectZ.Data
{
    public class CharacterAnimationData
    {
        // params
        private string _animParamHorizontalValue = "HorizontalValue";
        private string _animParamVerticalValue = "VerticalValue";

        // anim name
        private string _animNameLocomotion = "Locomotion";
        private string _animNameRollForward = "Roll_Forward";
        private string _animNameBackStep = "Back_Step";

        private string _animNameJump = "Jump";
        private string _animNameFalling = "Falling";
        private string _animNameLanding = "Landing";

        private string _animNameLightAttack01 = "Light_Attack_01";
        private string _animNameLightAttack02 = "Light_Attack_02";

        private string _animNameGetDamageFront = "Get_Damage_F";
        private string _animNameGetDamageBack = "Get_Damage_B";
        private string _animNameGetDamageRight = "Get_Damage_R";
        private string _animNameGetDamageLeft = "Get_Damage_L";

        private string _animNameDeath = "Death";

        // param properties
        public int AnimParamHorizontalValue { get; private set; }
        public int AnimParamVerticalValue { get; private set; }

        // anim name properties
        public int AnimNameLocomotion { get; private set; }
        public int AnimNameRollForward { get; private set; }
        public int AnimNameBackStep { get; private set; }

        public int AnimNameJump { get; private set; }
        public int AnimNameFalling { get; private set; }
        public int AnimNameLanding { get; private set; }

        public int AnimNameLightAttack01 { get; private set; }
        public int AnimNameLightAttack02 { get; private set; }

        public int AnimNameGetDamageFront { get; private set; }
        public int AnimNameGetDamageBack { get; private set; }
        public int AnimNameGetDamageRight { get; private set; }
        public int AnimNameGetDamageLeft { get; private set; }

        public int AnimNameDeath { get; private set; }

        public virtual void InitializeData()
        {
            AnimParamHorizontalValue = Animator.StringToHash(_animParamHorizontalValue);
            AnimParamVerticalValue = Animator.StringToHash(_animParamVerticalValue);

            AnimNameLocomotion = Animator.StringToHash(_animNameLocomotion);
            AnimNameRollForward = Animator.StringToHash(_animNameRollForward);
            AnimNameBackStep = Animator.StringToHash(_animNameBackStep);

            AnimNameJump = Animator.StringToHash(_animNameJump);
            AnimNameFalling = Animator.StringToHash(_animNameFalling);
            AnimNameLanding = Animator.StringToHash(_animNameLanding);

            AnimNameLightAttack01 = Animator.StringToHash(_animNameLightAttack01);
            AnimNameLightAttack02 = Animator.StringToHash(_animNameLightAttack02);

            AnimNameGetDamageFront = Animator.StringToHash(_animNameGetDamageFront);
            AnimNameGetDamageBack = Animator.StringToHash(_animNameGetDamageBack);
            AnimNameGetDamageRight = Animator.StringToHash(_animNameGetDamageRight);
            AnimNameGetDamageLeft = Animator.StringToHash(_animNameGetDamageLeft);

            AnimNameDeath = Animator.StringToHash(_animNameDeath);
        }
    }
}
