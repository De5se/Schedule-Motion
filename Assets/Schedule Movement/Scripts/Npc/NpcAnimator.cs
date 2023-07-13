using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimator : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController walkAnimation;
    [SerializeField] private AnimatorOverrideController runAnimation;
    [SerializeField] private AnimatorOverrideController[] defaultAnimations;
    
    [SerializeField]
    private Animator npcAnimator;
    private readonly int _offset = Animator.StringToHash("Offset");

    private Coroutine _changingToIdleAnimationCoroutine;
    
    private AnimatorOverrideController DefaultAnimatorController => defaultAnimations.Length > 0
        ? defaultAnimations[Random.Range(0, defaultAnimations.Length)]
        : null;
    
    private void Awake()
    {
        npcAnimator.SetFloat(_offset, Random.Range(0f, 1f));
        SetIdle();
    }

    public void SetIdle()
    {
        UpdateAnimatorController(DefaultAnimatorController, true);
    }
    
    public void SetWalking()
    {
        UpdateAnimatorController(walkAnimation);
    }
    
    public void SetRunning()
    {
        UpdateAnimatorController(runAnimation);
    }

    public void UpdateAnimatorController(RuntimeAnimatorController animatorOverrideController, bool finishWithIdle = false)
    {
        if (animatorOverrideController == null)
        {
            animatorOverrideController = DefaultAnimatorController;
            finishWithIdle = true;
        }
        npcAnimator.runtimeAnimatorController = animatorOverrideController;
        
        if (_changingToIdleAnimationCoroutine != null){
            StopCoroutine(_changingToIdleAnimationCoroutine);
        }
        if (finishWithIdle)
        {
            //_changingToIdleAnimationCoroutine = StartCoroutine(ChangeToIdle());
        }
    }

    private IEnumerator ChangeToIdle()
    {
        var stateInfo = npcAnimator.GetCurrentAnimatorStateInfo(0);
        var duration = stateInfo.length;
        yield return new WaitForSeconds(duration);
        SetIdle();
    }
}
