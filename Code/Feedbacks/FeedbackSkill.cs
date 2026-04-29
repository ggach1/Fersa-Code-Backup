using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace CIW.Code.Feedbacks
{
    public class FeedbackSkill : MonoBehaviour
    {
        [Header("1. Zoom In Phase")]
        [SerializeField] float zoomDuration = 0.3f;
        [SerializeField] float pauseAfterZoom = 0.3f;

        [Header("2. Attack Phase")]
        [SerializeField] float hitDelay = 0.4f;
        [SerializeField] float pauseAfterHit = 0.5f;

        [Header("3. Zoom Out Phase")]
        [SerializeField] float shrinkDuration = 0.3f;
        [SerializeField] float pauseAfterShrink = 0.2f;

        [SerializeField] FeedbackOverride[] feedbackOverrides;
        List<FeedbackData> _activeFeedbacks = new List<FeedbackData>();

        private void Awake() => InitFeedback();

        private void InitFeedback()
        {
            if (feedbackOverrides == null) return;
            _activeFeedbacks = feedbackOverrides.Select(ov => ov.CreateFeedback()).Where(fb => fb != null).ToList();
        }

        public void Execute(bool isChain, Transform target, bool doZoomIn, bool doZoomOut, Action onZoomComplete, Action onHit, Action onShrinkComplete, Action onComplete)
        {
            StartCoroutine(SequenceRoutine(isChain, target, doZoomIn, doZoomOut, onZoomComplete, onHit, onShrinkComplete, onComplete));
        }

        private IEnumerator SequenceRoutine(bool isChain, Transform target, bool doZoomIn, bool doZoomOut, Action onZoomComplete, Action onHit, Action onShrinkComplete, Action onComplete)
        {
            if (doZoomIn)
            {
                foreach (var fb in _activeFeedbacks)
                    if (fb != null && fb.ShouldPlay(isChain)) fb.PlayFeedback(target);
                yield return new WaitForSeconds(zoomDuration);
                yield return new WaitForSeconds(pauseAfterZoom);
            }
            
            onZoomComplete?.Invoke(); 

            yield return new WaitForSeconds(hitDelay);
            onHit?.Invoke(); 

            yield return new WaitForSeconds(pauseAfterHit);

            if (doZoomOut)
            {
                foreach (var fb in _activeFeedbacks)
                    if (fb != null) fb.StopFeedback(null);
                yield return new WaitForSeconds(shrinkDuration);
                yield return new WaitForSeconds(pauseAfterShrink);
            }
            
            onShrinkComplete?.Invoke();

            onComplete?.Invoke();
        }

        public void ForceZoomOut(Action onShrinkComplete)
        {
            StartCoroutine(ForceZoomOutRoutine(onShrinkComplete));
        }

        private IEnumerator ForceZoomOutRoutine(Action onShrinkComplete)
        {
            foreach (var fb in _activeFeedbacks)
                if (fb != null) fb.StopFeedback(null);
            yield return new WaitForSeconds(shrinkDuration);
            yield return new WaitForSeconds(pauseAfterShrink);
            onShrinkComplete?.Invoke();
        }
        
    }
}