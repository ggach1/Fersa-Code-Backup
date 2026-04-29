using UnityEngine;

namespace CIW.Code.Player.SkillCompo
{
    public class GameSystemInitializer : MonoBehaviour
    {
        [SerializeField] PlayerSkillComponent playerSkillComponent;
        [SerializeField] RecipeSystemAdapter recipeAdapter;

        private void Awake()
        {
            if (playerSkillComponent != null && recipeAdapter != null)
            {
                playerSkillComponent.Initialize(recipeAdapter);
                Debug.Log("<color=green>시동 완료:</color> PlayerSkillComponent가 성공적으로 초기화되었습니다.");
            }
            else
                Debug.LogError("초기화 실패: PlayerSkillComponent 또는 RecipeSystemAdapter가 할당되지 않았습니다.");
        }
    }
}

