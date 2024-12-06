using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();


    /// <summary>
    /// �o�ߐ��K���Ԃ�Ԃ��܂��A�J�ڂ��Ă���ꍇ�͑J�ڐ�A�j���[�V�����̌o�ߎ��Ԃ��Ԃ���܂�
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName"></param>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    protected float GetAnimationNormalizedTime(Animator animator,string tagName = null, int layerIndex = 0)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            if (animator.IsInTransition(layerIndex))
            {
                return animator.GetNextAnimatorStateInfo(layerIndex).normalizedTime;
            }
            else
            {
                return animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
            }
        }
        else
        {
            int tagNameHash = Animator.StringToHash(tagName);
            if (animator.IsInTransition(layerIndex))
            {
                AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(layerIndex);
                if(stateInfo.tagHash == tagNameHash) { return stateInfo.normalizedTime; }
                else { return -1; }
            }
            else
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                if (stateInfo.tagHash == tagNameHash) { return stateInfo.normalizedTime; }
                else { return -1; }
            }
        }

        
    }
    
}
