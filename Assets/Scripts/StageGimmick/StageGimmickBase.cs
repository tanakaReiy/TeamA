using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageGimmickBase : MonoBehaviour, ISendCompleteFlag
{
    /// <summary>
    /// <para>クリア時に呼び出す処理</para>
    /// <para>※overrideする場合は必ずbase.ClearAction();を含んでください</para>
    /// </summary>
    protected virtual void ClearAction()
    {
        SendFlag();
    }

    public void SendFlag()
    {
        
    }
}
