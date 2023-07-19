using UnityEngine;

namespace Schedule_Movement.Scripts.Npc.Fighting
{
    public interface IFightingUnit
    {
        public Transform Transform { get; }
        public FightingNpc FightingNpc => Transform.GetComponent<FightingNpc>();
        public bool IsAlive => FightingNpc.IsAlive;
        public void StartFighting();
    }
}