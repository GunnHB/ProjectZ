using UnityEngine;

namespace ProjectZ.UI
{
    [RequireComponent(typeof(Animator))]
    public class InventoryPlayerControls : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            InitAnimData();
        }

        private void InitAnimData()
        {

        }
    }
}
