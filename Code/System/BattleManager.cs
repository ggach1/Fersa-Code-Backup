using CIW.Code.System.Events;
using PSB.Code.BattleCode.UIs;
using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using UnityEngine;
using Work.PSB.Code.FieldCode.MapSaves;

namespace CIW.Code.System
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private VictoryLootPanelUI victoryPanel;
        [SerializeField] private FailLootPanelUI failPanel;

        private bool _handled;

        private void Awake()
        {
            Bus<BattleEnd>.OnEvent += HandleBattleEnd;
        }

        private void OnDestroy()
        {
            Bus<BattleEnd>.OnEvent -= HandleBattleEnd;
        }

        private void HandleBattleEnd(BattleEnd evt)
        {
            if (_handled) return;
            _handled = true;
            
            string returnScene = null;
            if (BattleContext.HasContext)
                returnScene = BattleContext.FieldSceneName;
            
            if (BattleContext.HasContext && evt.IsVictory)
            {
                SceneSaveSystem.SetEnemyAlive(
                    BattleContext.FieldSceneName,
                    BattleContext.FieldEnemyId,
                    isAlive: false
                );
            }
            
            if (!string.IsNullOrEmpty(returnScene))
            {
                victoryPanel.SetReturnScene(returnScene);
                failPanel?.SetReturnScene(returnScene);
            }
            
            BattleContext.Clear();
            
            if (evt.IsVictory) StartCoroutine(victoryPanel.ShowCoroutine());
            else StartCoroutine(failPanel?.ShowCoroutine());
        }
        
    }
}