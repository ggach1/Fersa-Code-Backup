using PSB.Code.CoreSystem.Events;
using PSW.Code.EventBus;
using UnityEngine;

namespace CIW.Roulette
{
    public class RouletteManager : MonoBehaviour
    {
        [SerializeField] GameObject roulettePanel;

        [SerializeField] string rewardKey;

        private void Awake()
        {
            Bus<TalkFinished>.OnEvent += HandleTalkFinished;
        }

        private void OnDestroy()
        {
            Bus<TalkFinished>.OnEvent -= HandleTalkFinished;
        }

        private void HandleTalkFinished(TalkFinished e)
        {
            roulettePanel.SetActive(true);
        }
    }
}

