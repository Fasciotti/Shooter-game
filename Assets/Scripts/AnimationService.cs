using UnityEngine;
using System.Threading.Tasks;
using System;

public class AnimationService : MonoBehaviour
{
    public static async Task PlayAnimation(Animator animator, string stateName, Action<Animator, string> setParameter, int layer = 0)
    {
        int stateHash = Animator.StringToHash(stateName);

        // Start the animation
        setParameter(animator, stateName);

        // Wait until state is entered
        while (animator.GetCurrentAnimatorStateInfo(layer).shortNameHash != stateHash)
            await Task.Yield();

        // Wait until state exits
        while (animator.GetCurrentAnimatorStateInfo(layer).shortNameHash == stateHash)
            await Task.Yield();
    }

    public static async Task AnimationTriggerReturnFinished(Animator animator, string stateName, int layer = 0) =>
        await PlayAnimation(animator, stateName, (a, s) => a.SetTrigger(s), layer);

    public static async Task AnimationBoolReturnFinished(Animator animator, string stateName, int layer = 0) =>
        await PlayAnimation(animator, stateName, (a, s) => a.SetBool(s, true), layer);
}