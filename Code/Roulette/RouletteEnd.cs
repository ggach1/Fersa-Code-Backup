using PSW.Code.EventBus;
using UnityEngine;
using Work.PSB.Code.FieldCode.MapSaves;

namespace CIW.Roulette
{
    public struct RouletteEndEvent : IEvent {}

    public class RouletteEnd : MonoBehaviour
    {
        [SerializeField] private NormalPortalEntity normalPortalEntity;
        [SerializeField] GameObject roulette;
        [SerializeField] GameObject roulettePanel;
        
        private void OnEnable()
        {
            roulette.SetActive(true);
            roulettePanel.SetActive(false);
            Bus<RouletteEndEvent>.OnEvent += HandleRouletteEnd;
        }

        private void OnDisable()
        {
            Bus<RouletteEndEvent>.OnEvent -= HandleRouletteEnd;
        }

        private void HandleRouletteEnd(RouletteEndEvent e)
        {
            roulettePanel.SetActive(false);
            roulette.SetActive(false);
            normalPortalEntity.Open();
        }
        
    }
}

