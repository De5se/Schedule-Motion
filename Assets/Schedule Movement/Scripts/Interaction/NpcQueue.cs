using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class NpcQueue : QueueBase
    {
        private List<Transform> _waitPoints;
        
        protected override void Start()
        {
            _waitPoints = transform.GetComponentsInChildren<Transform>().ToList();
        }

        public override void AddNpc(NPC npc)
        {
            base.AddNpc(npc);
            npc.SetDestination(_waitPoints[Queue.Count].position);
        }

        public override NPC GetNpc()
        {
            var npc = base.GetNpc();

            return npc;
        }

        private void MoveQueue()
        {
            for (var index = 0; index < Queue.Count; index++)
            {
                var patient = Queue[index];
                patient.SetDestination(_waitPoints[index].position);
            }
        }
    }
}