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
            Empty,
            AQuarter,
            Half,
            ThreeQaurter,
            Full,
        }

        [Title(TITLE_HEART_IMAGE)]
        [SerializeField] private List<Image> _heartObjList;

        private HeartType _heartType;

        private void Awake()
        {
            // 일단 다 끄기
            foreach (var item in _heartObjList)
                item.gameObject.SetActive(false);
        }

        public void SetHeart(HeartType type)
        {
            _heartType = type;

            if (_heartType == HeartType.None || _heartType == HeartType.Empty)
                return;

            for (int index = 0; index < (int)_heartType; index++)
                _heartObjList[index].gameObject.SetActive(true);
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

            for (int index = 0; index < (int)targetType; index++)
                sequence.Append(DOTween.To(() => 0f, x => _heartObjList[index].gameObject.SetActive(false), 0f, .2f));

            return sequence;
        }
    }
}
