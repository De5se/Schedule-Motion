using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class UsingMaterialsRoom : InteractionRoom
    {
        [Space(10)] 
        [ShowInInspector, ReadOnly]
        private int _materialsLeft;
        
        public void AddMaterials(int materials)
        {
            _materialsLeft += materials;
        }

        public void ClearMaterialsCount()
        {
            _materialsLeft = 0;
        }
    }
}