using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class UsingMaterialsRoom : InteractionRoom
    {
        [Space(10)] 
        [ShowInInspector, ReadOnly]
        protected int MaterialsLeft;
        
        [ShowInInspector, ReadOnly] 
        private UsingMaterialItem[] _usingItems;

        protected override void Awake()
        {
            base.Awake();
            _usingItems = GetComponentsInChildren<UsingMaterialItem>(true);
        }

        protected override void Start()
        {
            base.Start();

            foreach (var item in _usingItems)
            {
                item.OnInteractionFinished += UseMaterial;
            }
        }

        public void AddMaterials(int materials)
        {
            MaterialsLeft += materials;
        }

        public override void ClearPatientQueue()
        {
            base.ClearPatientQueue();
            ClearMaterialsCount();

            employeeQueue.ClearQueue();
        }


        private void UseMaterial()
        {
            MaterialsLeft--;
        }
        
        private void ClearMaterialsCount()
        {
            MaterialsLeft = 0;
        }
    }
}