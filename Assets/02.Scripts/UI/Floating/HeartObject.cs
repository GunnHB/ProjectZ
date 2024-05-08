using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;

using DG.Tweening;

namespace ProjectZ.UI
{
    public class HeartObject : MonoBehaviour
    {
        private const string TITLE_HEART_IMAGE = "[Heart Image]";
        public enum HeartType
        {
            None = -1,
            AQuarter,
            Half,
            ThreeQaurter,
            Full,
        }

        [Title(TITLE_HEART_IMAGE)]
        [SerializeField] private List<Image> _heartObjList;

        private HeartType _heartType;

        private Dictionary<HeartType, Image> _heartDictionary;

        private Coroutine _activeHeartCoroutine;

        public bool IsEmpty => _heartType == HeartType.None;
        public bool IsFull => _heartType == HeartType.Full;

        private void Awake()
        {
            // 딕셔너리 등록
            _heartDictionary = new()
            {
                {HeartType.AQuarter, _heartObjList[0]},
                {HeartType.Half, _heartObjList[1]},
                {HeartType.ThreeQaurter, _heartObjList[2]},
                {HeartType.Full, _heartObjList[3]},
            };

            // 일단 다 끄기
            foreach (var item in _heartObjList)
                item.gameObject.SetActive(false);
        }

        public void SetHeart(HeartType type)
        {
            _heartType = type;

            if (_heartType == HeartType.None)
                return;

            for (int index = 0; index < (int)_heartType + 1; index++)
                _heartObjList[index].gameObject.SetActive(true);
        }

        public void ActiveHeartByOne(bool active)
        {
            if (active)
            {
                if (IsFull)
                    return;

                _heartType += 1;
                _heartDictionary[_heartType].gameObject.SetActive(true);
            }
            else
            {
                if (IsEmpty)
                    return;

                _heartDictionary[_heartType].gameObject.SetActive(false);
                _heartType -= 1;
            }
        }
    }
}
