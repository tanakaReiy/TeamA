using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

[Obsolete]
public class CapturableDetector : OnceDetector<Capturable>
{
   
}

namespace AnimaitonEventReceivable
{
    public interface ICaptureAnimationEventReceivable
    {
        void EnableDetection();
        void DisableDetection();
    }
}