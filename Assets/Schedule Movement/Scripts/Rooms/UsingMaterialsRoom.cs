using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class UsingMaterialsRoom : InteractionRoom
    {
        [Space(10)] 
        [ShowInInspector, ReadOnly]
        private int _materialsLeft;
        
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
            _materialsLeft += materials;
        }

        public void ClearMaterialsCount()
        {
            _materialsLeft = 0;
        }


        private void UseMaterial()
        {
            _materialsLeft--;
        }
    }
}