using System;
using System.Collections;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Schedule_Movement.Scripts
{
    public class InteractionPoint : MonoBehaviour
    {
        [SerializeField] private Transform employeeInteractionPoint;
        [SerializeField] private bool setEmployeeFree;
        [SerializeField] private Transform chronosInteractionPoint;
        [SerializeField] private bool setChronosFree;

        [ShowInInspector, ReadOnly] private ChronosBehaviour _currentChronos;
        [ShowInInspector, ReadOnly] private Employee _currentEmployee;

        private InteractionItem _interactionItem;
        
        private const float InteractionDuration = 5f;

        private Coroutine _interactionCoroutine;

        private InteractionType _interactionType;
        
        [ShowInInspector, ReadOnly]
        protected Employee LastEmployee;
        [ShowInInspector, ReadOnly]
        protected ChronosBehaviour LastPatient;

        public event Action OnFree;

        public bool SetChronosFree => setChronosFree;

        public bool SetEmployeeFree => setEmployeeFree;

        private void Awake()
        {
            _interactionItem = GetComponentInParent<InteractionItem>(true);
            _interactionType = _interactionItem.InteractionType;
        }

        public bool HasNpcPlace(InteractionType targetType)
        {
            switch (targetType)
            {
                case InteractionType.Agent:
                    return _currentEmployee == null && (_interactionType == InteractionType.Agent ||
                                                        (_currentChronos != null &&
                                                         _interactionType == InteractionType.Both));
                case InteractionType.Chronos:
                    return _currentChronos == null &&
                           _interactionType is InteractionType.Chronos or InteractionType.Both;
                case InteractionType.Both:
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        public void SetAgent(Employee employee)
        {
            _currentEmployee = employee;
            
            _currentEmployee.SetInteraction(this, employeeInteractionPoint.position);

            _currentEmployee.DestinationReachedAction += StartInteraction;
        }
        
        public void SetChronos(ChronosBehaviour chronos)
        {
            _currentChronos = chronos;
            _currentChronos.SetInteraction(this, chronosInteractionPoint.position);

            if (_interactionType == InteractionType.Chronos)
            {
                _currentChronos.DestinationReachedAction += StartInteraction;
            }
            else
            {
                _currentChronos.DestinationReachedAction += _interactionItem.AddItemWithoutEmployee;
            }
        }


        protected virtual void StartInteraction()
        {
            _interactionCoroutine = StartCoroutine(Interaction());
        }

        private IEnumerator Interaction()
        {
            Debug.Log($"{transform.parent.name} interaction started");
            LastEmployee = _currentEmployee;
            LastPatient = _currentChronos;
            yield return new WaitForSeconds(InteractionDuration);
            FinishInteraction();
        }

        protected virtual void FinishInteraction()
        {
            if (_currentChronos != null) _currentChronos.FinishInteraction();
            if (_currentEmployee != null) _currentEmployee.FinishInteraction();

            _currentChronos = null;
            _currentEmployee = null;
            OnFree?.Invoke();
        }
    }
}