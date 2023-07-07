using System;
using System.Collections;
using System.Collections.Generic;
using Schedule_Movement.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;

    [SerializeField]
    private InteractionPoint currentInteraction;
    
    private Coroutine _destinationCoroutine;
    private Vector3 _navMeshDestination;
    
    public event Action DestinationReachedAction;
    public event Action OnFree;
    
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
        yield return new WaitUntil(IsAgentStopped);
        Debug.Log($"{gameObject.name} reached destination");
        DestinationReachedAction?.Invoke();
    }

    public void SetStoppingDistance(float stoppingDistance) => NavMeshAgent.stoppingDistance = stoppingDistance;
    #endregion


    public virtual void SetInteraction(InteractionPoint interactionPoint, Vector3 targetPosition)
    {
        currentInteraction = interactionPoint;
        SetDestination(targetPosition);
    }
    
    public void FinishInteraction()
    {
        currentInteraction = null;
    }

    public void StopInteraction()
    {
        
    }

    public void InvokeFreeAction()
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
}
