using System;
using UnityEngine;

namespace CIW.Code.Feedbacks
{
    [Serializable]
    public class FeedbackOverride
    {
        public FeedbackData feedback;
        public bool isUseOverride;

        [Header("Condition Override")]
        public bool playOnChain = true;
        public bool playOnNormal = true;

        [SerializeReference]
        public FeedbackConfig customConfig;

        public FeedbackData CreateFeedback()
        {
            if (feedback == null || !isUseOverride) return null;

            FeedbackData cloned = UnityEngine.Object.Instantiate(feedback);

            cloned.playOnChain = playOnChain;
            cloned.playOnNormal = playOnNormal;

            if (customConfig != null)
                cloned.ApplyConfig(customConfig);

            return cloned;
        }
    }
}

