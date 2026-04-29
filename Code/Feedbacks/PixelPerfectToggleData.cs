using PSW.Code.EventBus;
using System;
using UnityEngine;
using YIS.Code.Feedbacks;
using YIS.Code.Feedbacks.Managers;

namespace CIW.Code.Feedbacks
{
    [Serializable]
    public class PixelPerfectConfig : FeedbackConfig { }

    [CreateAssetMenu(fileName = "PixelPerfect Data", menuName = "SO/Feedback/PixelPerfect Toggle")]
    public class PixelPerfectToggleData : FeedbackData
    {
        public override Type ConfigType => typeof(PixelPerfectConfig);

        public override FeedbackConfig CreateDefaultConfig()
        {
            return new PixelPerfectConfig
            {};
        }

        public override void PlayFeedback(Transform target) => Bus<PixelPerfectEvent>.Raise(new PixelPerfectEvent(false));

        public override void StopFeedback(Transform target) => Bus<PixelPerfectEvent>.Raise(new PixelPerfectEvent(true));
    }
}

