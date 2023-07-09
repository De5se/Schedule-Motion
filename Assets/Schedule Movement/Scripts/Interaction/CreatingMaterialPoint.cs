using System;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class CreatingMaterialPoint : InteractionPoint
    {
        public event Action OnCreated;
        
        protected override void FinishInteraction()
        {
            base.FinishInteraction();
            OnCreated?.Invoke();
        }
    }
}