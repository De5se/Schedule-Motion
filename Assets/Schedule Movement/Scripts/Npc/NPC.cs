using System;
using System.Collections;
using Schedule_Movement.Scripts.Environment.Items;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    [SerializeField] private NpcAnimator npcAnimator;

    [ShowInInspector, ReadOnly]
    protected InteractionPoint CurrentInteraction;
    
    private Coroutine _destinationCoroutine;
    private Vector3 _navMeshDestination;
    
    public event Action DestinationReachedAction;
    public event Action OnFree;
    public event Action<NPC> OnDestroyed;
    
    public void UpdateAnimatorController(RuntimeAnimatorController animatorOverrideController,
        bool finishWithIdle = false) =>
        npcAnimator.UpdateAnimatorController(animatorOverrideController, finishWithIdle);
    
    private bool IsAgentStopped() =>
        Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
            new Vector2(_navMeshDestination.x, _navMeshDestination.z)) <= NavMeshAgent.stoppingDistance
        && NavMeshAgent.pathPending == false;
    
    #region NavMeshAgent Control
    public void SetDestination(Vector3 targetPosition)
    {
        DestinationReachedAction = null;
        if (!NavMeshAgent.isOnNavMesh)
        {
            SetToNavMesh();
        }

        _navMeshDestination = targetPosition;
        NavMeshAgent.SetDestination(_navMeshDestination);

        if (_destinationCoroutine != null)
        {
            StopCoroutine(_destinationCoroutine);
        }

        _destinationCoroutine = StartCoroutine(DestinationReachedCoroutine());
    }
        
    private IEnumerator DestinationReachedCoroutine()
    {
        npcAnimator.SetWalking();
        
        yield return new WaitUntil(IsAgentStopped);
        Debug.Log($"{gameObject.name} reached destination");
        npcAnimator.SetIdle();
        DestinationReachedAction?.Invoke();
    }

    public void SetStoppingDistance(float stoppingDistance) => NavMeshAgent.stoppingDistance = stoppingDistance;
    #endregion
    
    public virtual void SetInteraction(InteractionPoint interactionPoint, Vector3 targetPosition)
    {
        CurrentInteraction = interactionPoint;
        SetDestination(targetPosition);
    }
    
    public virtual void FinishInteraction()
    {
        CurrentInteraction = null;
    }

    public void StopInteraction()
    {
        
    }

    protected virtual void InvokeFreeAction()
    {
        OnFree?.Invoke();
    }
    
    #region Set to transform
    public void SetToTransform(Transform targetTransform)
    {
        NavMeshAgent.enabled = false;
        var transform1 = transform;
        transform1.position = targetTransform.position;
        transform1.rotation = targetTransform.rotation;
    }
        
    private void SetToNavMesh()
    {
        if (NavMesh.SamplePosition(transform.position, out var hit, 2.0f, NavMesh.AllAreas))
        {
            NavMeshAgent.Warp(hit.position);
            NavMeshAgent.enabled = true;
        }
        else
        {
            Debug.LogWarning("Didn't found nearest NavMesh position");
        }
    }
    #endregion

    public virtual void DestroyNpc()
    {
        StopAllCoroutines();
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
