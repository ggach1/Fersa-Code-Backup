using CIW.Code.System.Events;
using PSW.Code.EventBus;
using System;
using UnityEngine;

namespace CIW.Code.Feedbacks
{
    [Serializable]
    public class PlayerMoveConfig : FeedbackConfig
    {
        public float BackTime;
        public float DashTime;
    }

    [CreateAssetMenu(fileName = "Player Move Data", menuName = "SO/Feedback/PlayerMove")]
    public class PlayerMoveData : FeedbackData
    {
        public float CurrentBackTime { get; private set; } = 0.07f;
        public float CurrentDashTime { get; private set; } = 0.04f;

        public override Type ConfigType => typeof(PlayerMoveConfig);

        public override FeedbackConfig CreateDefaultConfig()
        {
            return new PlayerMoveConfig
            {
                BackTime = CurrentBackTime,
                DashTime = CurrentDashTime,
            };
        }

        public override void ApplyConfig(FeedbackConfig config)
        {
            if (config is PlayerMoveConfig moveConfig)
            {
                CurrentBackTime = moveConfig.BackTime;
                CurrentDashTime = moveConfig.DashTime;
            }
        }

        public override void PlayFeedback(Transform target)
        {

        }
            //=> Bus<MoveControlEvent>.Raise(new MoveControlEvent(CurrentBackTime, CurrentDashTime));

        public override void StopFeedback(Transform target)
        {
            
        }
    }
}

