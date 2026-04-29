using System;
using UnityEngine;

namespace CIW.Code.Feedbacks
{
    [Serializable]
    public abstract class FeedbackConfig { }

    public abstract class FeedbackData : ScriptableObject
    {
        //[SerializeReference]
        //public FeedbackConfig customConfig; // Config 없이 리플렉션으로 저장이 될까...?

        public bool playOnChain = true; // 체이닝 되었을 때 실행 여부
        public bool playOnNormal = true; // 체이닝 발동 안 되었을 때 실행 여부

        public bool ShouldPlay(bool isChain) => isChain ? playOnChain : playOnNormal;

        // Feedback Data에 대응하는 Config Type
        // 없으면 null 반환할거임.
        public abstract Type ConfigType { get; }

        // 인스펙터에서 Create 버튼을 눌렀을 때, 기본 Config를 생성한다.
        // 기본값을 현재 SO 값 기준으로 넣고 싶으면 여기에 넣으셈
        public virtual FeedbackConfig CreateDefaultConfig() => null;

        public abstract void PlayFeedback(Transform target);
        public abstract void StopFeedback(Transform target);

        public virtual void ApplyConfig(FeedbackConfig config) { }
    }
}
    
