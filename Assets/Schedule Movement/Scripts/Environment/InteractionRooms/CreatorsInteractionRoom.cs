using Schedule_Movement.Scripts.Environment.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Environment.InteractionRooms
{
    public class CreatorsInteractionRoom : InteractionRoom
    {
        [field: Space(10)]
        [ShowInInspector, ReadOnly] 
        public int CreatedItems { get; private set; }
        [ShowInInspector, ReadOnly] 
        private CreatingMaterialsItem[] _creatingItems;

        protected override void Awake()
        {
            base.Awake();
            _creatingItems = GetComponentsInChildren<CreatingMaterialsItem>(true);
        }

        protected override void Start()
        {
            base.Start();

            foreach (var item in _creatingItems)
            {
                item.OnInteractionFinished += AddItem;
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