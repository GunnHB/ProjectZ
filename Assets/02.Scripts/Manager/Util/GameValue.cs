using System.Runtime.Serialization;

namespace ProjectZ.Manager
{
    public static class GameValue
    {
        public enum WeaponType
        {
            [EnumMember(Value = "None")]
            None = -1,
            [EnumMember(Value = "OneHand")]
            NoWeapon,
            [EnumMember(Value = "OneHand")]
            OneHand,
            [EnumMember(Value = "TwoHand")]
            TwoHand,
        }

        public enum ItemType
        {
            [EnumMember(Value = "None")]
            None = -1,
            [EnumMember(Value = "Weapon")]
            Weapon,
            [EnumMember(Value = "Shield")]
            Shield,
            [EnumMember(Value = "Bow")]
            Bow,
            [EnumMember(Value = "Armor")]
            Armor,
            [EnumMember(Value = "Food")]
            Food,
            [EnumMember(Value = "Default")]
            Default,
        }

        public enum StatsType
        {
            [EnumMember(Value = "None")]
            None = -1,
            [EnumMember(Value = "HP")]
            HP,
            [EnumMember(Value = "Stamina")]
            Stamina,
        }

        public readonly static float STAMINA_REDUCE = .7f;
        public readonly static float STAMINA_CHARGE = 1f;

        public readonly static float STAMINA_REDUCE_IMMEDIATE = 30f;

        public readonly static float STAMINA_DELAY_TIME = 2f;

        public readonly static float GRAVITY = -9.81f;

        public readonly static int LAYER_PLAYER = UnityEngine.LayerMask.NameToLayer("Player");
        public readonly static int LAYER_ENEMY = UnityEngine.LayerMask.NameToLayer("Enemy");

        // 아이템 타입에 따른 인벤토리의 초기 슬롯 수
        public readonly static int INVEN_WEAPON_SLOT_AMOUNT = 4;
        public readonly static int INVEN_SHIELD_SLOT_AMOUNT = 4;
        public readonly static int INVEN_BOW_SLOT_AMOUNT = 8;
        public readonly static int INVEN_ARMOR_SLOT_AMOUNT = 4;
        public readonly static int INVEN_FOOD_SLOT_AMOUNT = 30;
        public readonly static int INVEN_DEFAULT_SLOT_AMOUNT = 30;

        // 한 줄에 들어가는 슬롯 수
        public readonly static int INVEN_ROW_AMOUNT = 6;
    }
}