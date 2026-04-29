using UnityEngine;

namespace CIW.Code.FSM
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/StateData", order = 0)]
    public class StateDataSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public int stateIndex;
        public string animParamName;
        public int animationHash;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animParamName);
        }
    }
}