using PSW.Code.EventBus;
using System;
using UnityEngine;
using YIS.Code.Feedbacks;
using YIS.Code.Feedbacks.Managers;

namespace CIW.Code.Feedbacks
{
    public struct CameraMoveEvent : IEvent
    {
        public bool isStart;
        public Transform target;
        public float distance;
        public Vector3 offset;

        public CameraMoveEvent(bool isStart, Transform target, float distance, Vector3 offset)
        {
            this.isStart = isStart;
            this.target = target;
            this.distance = distance;
            this.offset = offset;
        }
    }

    [Serializable]
    public class CameraConfig : FeedbackConfig
    {
        public float distance;
        public Vector3 offset;
    }

    [CreateAssetMenu(fileName = "Camera Move Data", menuName = "SO/Feedback/CameraMove")]
    public class CameraMoveData : FeedbackData
    {
        public float distance = 5f;
        public Vector3 offset;

        public override Type ConfigType => typeof(CameraConfig);

        public override FeedbackConfig CreateDefaultConfig()
        {
            return new CameraConfig
            {
                distance = distance,
                offset = offset
            };
        }

        public override void ApplyConfig(FeedbackConfig config)
        {
            if (config is CameraConfig camConfig)
            {
                // 이렇게 하면 오버라이드가 잘 적용되는가
                this.distance = camConfig.distance;
                this.offset = camConfig.offset;
            }
        }

        public override void PlayFeedback(Transform target)
        {
            if (target == null) return;

            Bus<CameraMoveEvent>.Raise(new CameraMoveEvent(true, target, distance, offset));
        }

        public override void StopFeedback(Transform target)
        {
            Bus<CameraMoveEvent>.Raise(new CameraMoveEvent(false, target, distance, offset));
        }
    }
}

