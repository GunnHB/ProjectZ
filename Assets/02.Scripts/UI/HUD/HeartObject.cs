using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

namespace ProjectZ.UI
{
    public class HeartObject : MonoBehaviour
    {
        private const string TITLE_HEART_IMAGE = "[Heart Image]";
        public enum HeartType
        {
            None = -1,
            Empty,
            AQuarter,
            Half,
            ThreeQaurter,
            Full,
        }

        [Title(TITLE_HEART_IMAGE)]
        [SerializeField] private List<GameObject> _heartObjList;

        private HeartType _heartType;

        private void Awake()
        {
            // 일단 다 끄기
            foreach (var item in _heartObjList)
                item.SetActive(false);
        }

        public void SetHeart(HeartType type)
        {
            _heartType = type;

            if (_heartType == HeartType.None || _heartType == HeartType.Empty)
                return;

            for (int index = 0; index < (int)_heartType; index++)
                _heartObjList[index].SetActive(true);
        }
    }
}
