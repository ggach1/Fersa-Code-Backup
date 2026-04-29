using PSW.Code.EventBus;
using Unity.Cinemachine;
using UnityEngine;
using YIS.Code.Events;
using YIS.Code.Feedbacks;

namespace CIW.Code.Feedbacks
{
    public class TestChainFeedback : Feedback
    {
        [SerializeField] string message = "이 메시지는 체이닝 되었을 때만 출력됩니다.";

        public override void PlayFeedback()
        {
            Debug.Log($"Feedback : {message}");
        }

        public override void StopFeedback() { }
    }
}

