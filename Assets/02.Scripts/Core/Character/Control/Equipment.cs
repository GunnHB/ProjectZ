using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectZ.Core.Characters
{
    public class Equipment : MonoBehaviour
    {
        private const string TITLE_ONE_HAND = "[OneHand]";
        private const string TITLE_TWO_HAND = "[TwoHand]";
        private const string TITLE_SHIELD = "[Shield]";

        [Title(TITLE_ONE_HAND)]
        [SerializeField] private Transform _oneHandRighHandHolder;
        [SerializeField] private Transform _oneHandLeftHandHolder;
        [SerializeField] private Transform _oneHandRightHandSheathHolder;
        [SerializeField] private Transform _oneHandLeftHandSheathHolder;

        [Title(TITLE_TWO_HAND)]
        [SerializeField] private Transform _twoHandhHolder;
        [SerializeField] private Transform _twoHandSheahtHolder;

        [Title(TITLE_SHIELD)]
        [SerializeField] private Transform _shieldHandHolder;
        [SerializeField] private Transform _shieldSheathHolder;
    }
}
