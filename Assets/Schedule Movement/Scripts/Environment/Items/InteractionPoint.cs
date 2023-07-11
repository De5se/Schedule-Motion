using System;
using System.Collections;
using Schedule_Movement.Scripts.Npc.Agents;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Schedule_Movement.Scripts.Environment.Items
{
    public class InteractionPoint : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform employeeInteractionPoint;
        [SerializeField] private bool setEmployeeFree;
        [SerializeField] private bool finishEmployeeAnimationWithIdle;
        [SerializeField] private Transform chronosInteractionPoint;
        [SerializeField] private bool setChronosFree;

        [ShowInInspector, ReadOnly] private ChronosBehaviour _currentChronos;
        [ShowInInspector, ReadOnly] private Employee _currentEmployee;

        [ShowInInspector, ReadOnly] protected Employee LastEmployee;
        [ShowInInspector, ReadOnly] protected ChronosBehaviour LastPatient;

        #region Animations
        [Space(10)]
        [SerializeField] private bool leftPreviousAnimation;
        [SerializeField] private AnimatorOverrideController[] patientAnimations;
        [SerializeField] private AnimatorOverrideController[] patientWaitAnimations;
        [SerializeField] private AnimatorOverrideController[] employeeAnimations;
        
        private AnimatorOverrideController PatientAnimation => patientAnimations.Length > 0
            ? patientAnimations[Random.Range(0, patientAnimations.Length)]
            : null;
        private AnimatorOverrideController PatientWaitAnimation => patientWaitAnimations.Length > 0
            ? patientWaitAnimations[Random.Range(0, patientWaitAnimations.Length)]
            : null;
        private AnimatorOverrideController EmployeeAnimation => employeeAnimations.Length > 0
            ? employeeAnimations[Random.Range(0, employeeAnimations.Length)]
            : null;
        #endregion
        
        private InteractionItem _interactionItem;
        
        private const float InteractionDuration = 5f;

        private Coroutine _interactionCoroutine;

        private InteractionType _interactionType;
        
        public event Action OnFree;
        public event Action OnInteractionStarted;
        public event Action OnInteractionFinished;

        public bool SetChronosFree => setChronosFree;

        public bool SetEmployeeFree => setEmployeeFree;
        #endregion

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
            UpdateNpcAnimators();
            _interactionCoroutine = StartCoroutine(Interaction());
            OnInteractionStarted?.Invoke();
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
            OnInteractionFinished?.Invoke();
            OnFree?.Invoke();
        }
        
        private void UpdateNpcAnimators()
        {
            if (leftPreviousAnimation)
            {
                return;
            }

            if (_currentEmployee != null)
            {
                _currentEmployee.UpdateAnimatorController(EmployeeAnimation, finishEmployeeAnimationWithIdle);
            }
            if (_currentChronos != null)
            {
                _currentChronos.UpdateAnimatorController(PatientAnimation);
            }
        }
    }
}