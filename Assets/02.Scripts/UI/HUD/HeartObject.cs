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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                ActiveHeart(HeartType.AQuarter);
        }

        /// <summary>
        /// 빛나는 효과 시퀀스
        /// </summary>
        /// <param name="targetType">목표 타입</param>
        /// <returns></returns>
        public Sequence GlowSequence(HeartType targetType)
        {
            return DOTween.Sequence();
        }

        /// <summary>
        /// 활성화 / 비활성화 시퀀스
        /// </summary>
        /// <param name="targetType">목표 타입</param>
        /// <returns></returns>
        public Sequence ActiveSequence(HeartType targetType)
        {
            var sequence = DOTween.Sequence();

            for (int index = (int)HeartType.Full - 1; index >= (int)targetType; index--)
            {
                sequence.AppendCallback(() =>
                        {
                            _heartObjList[index].gameObject.SetActive(false);
                            Debug.Log($"{name}'s {_heartObjList[index]} is deactive");
                        })
                        .SetDelay(.05f);
            }

            return sequence;
        }

        public void ActiveHeart(HeartType targetType)
        {
            if (_activeHeartCoroutine != null)
            {
                StopCoroutine(_activeHeartCoroutine);
                _activeHeartCoroutine = null;
            }

            _activeHeartCoroutine = StartCoroutine(nameof(ActiveHeartCoroutine), targetType);
        }

        private IEnumerator ActiveHeartCoroutine(HeartType targetType)
        {
            HeartType tempType = HeartType.Full;

            while (true)
            {
                if (tempType == targetType)
                    yield break;

                _heartDictionary[tempType].gameObject.SetActive(false);
                tempType -= 1;

                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
