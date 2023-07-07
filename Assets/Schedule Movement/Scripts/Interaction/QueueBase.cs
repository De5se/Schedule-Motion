using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class QueueBase : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        protected readonly List<NPC> Queue = new();


        protected virtual void Start(){}

        public bool IsEmpty => Queue.IsNullOrEmpty();
        public bool ContainsNpc(NPC npc) => Queue.Contains(npc);
        
        public virtual void AddNpc(NPC npc)
        {
            Queue.Add(npc);
        }
        
        public virtual NPC GetNpc()
        {
            var npc = Queue[0];
            Queue.RemoveAt(0);
            return npc;
        }
    }
}