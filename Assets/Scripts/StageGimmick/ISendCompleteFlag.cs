using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ギミックがクリアした時に、フラグを通知する処理のインターフェイス
/// </summary>
public interface ISendCompleteFlag
{
    /// <summary>
    /// ギミックがクリアされたことを通知するメソッド
    /// </summary>
    void SendFlag();
}
