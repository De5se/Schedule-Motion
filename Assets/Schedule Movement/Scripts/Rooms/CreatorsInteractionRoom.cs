using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts.Rooms
{
    public class CreatorsInteractionRoom : InteractionRoom
    {
        [Space(10)]
        [ShowInInspector, ReadOnly] private int _createdItems;
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
            _createdItems++;
        }
    }
}