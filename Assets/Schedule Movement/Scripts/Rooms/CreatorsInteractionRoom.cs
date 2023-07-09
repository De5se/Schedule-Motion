using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class CreatorsInteractionRoom : InteractionRoom
    {
        [field: Space(10)]
        [ShowInInspector, ReadOnly] public int CreatedItems { get; private set; }
        private CreatingMaterialPoint[] _creatingPoints;
        
        protected override void Start()
        {
            base.Start();
            _creatingPoints = GetComponentsInChildren<CreatingMaterialPoint>(true);

            foreach (var point in _creatingPoints)
            {
                point.OnCreated += AddItem;
            }
        }
        
        private void AddItem()
        {
            CreatedItems++;
        }

        public void ClearMaterialsCount()
        {
            CreatedItems = 0;
        }
        
        public void TakeMaterials(int amount)
        {
            CreatedItems -= amount;
        }
    }
}