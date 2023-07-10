using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Schedule_Movement.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class FreeTimePointsQueue : QueueBase
{
    [SerializeField] 
    private Vector2 waitDuration = new(3f, 7f);
    private readonly List<Transform> _freePoints = new();
    private readonly Dictionary<NPC, Transform> _busyPoints = new();

    public event Action OnNewNpc;

    protected override void Awake()
    {
        AddPoints(transform);
    }

    public void AddPoints(Transform pointsParent)
    {
        _freePoints.AddRange(pointsParent.GetComponentsInChildren<Transform>());
        _freePoints.Remove(pointsParent);
    }

    public override void AddNpc(NPC npc)
    {
        base.AddNpc(npc);
        SetNpcToPoint(npc);
        OnNewNpc?.Invoke();
    }

    public override NPC GetNpc()
    {
        var npc =  base.GetNpc();
        _freePoints.Add(_busyPoints[npc]);
        _busyPoints.Remove(npc);
        return npc;
    }

    private void SetNpcToPoint(NPC npc)
    {
        var targetIndex = Random.Range(0, _freePoints.Count);
        var targetPoint = _freePoints[targetIndex];
        _freePoints.RemoveAt(targetIndex);
        
        _busyPoints[npc] = targetPoint;
        
        npc.SetDestination(targetPoint.position);
        npc.DestinationReachedAction += () =>
        {
            SetNpcToNextPoint(npc);
        };
    }

    private async void SetNpcToNextPoint(NPC npc)
    {
        var waitMilliseconds = (int) (1000 * Random.Range(waitDuration.x, waitDuration.y));
        await Task.Delay(waitMilliseconds);
        if (_busyPoints.ContainsKey(npc) == false)
        {
            return;
        }
        
        _freePoints.Add(_busyPoints[npc]);
        SetNpcToPoint(npc);
    }
}
