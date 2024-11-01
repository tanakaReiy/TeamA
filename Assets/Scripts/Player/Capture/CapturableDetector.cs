using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class CapturableDetector : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layer;
#if UNITY_EDITOR
    [SerializeField] private bool _showRange = false;
#endif


    private Detector<Capturable> _detector = new();
    private CancellationTokenSource _cts = null;
    public void StartDetection(Action<Capturable> onCapture)
    {
        _cts = new();
        FindCapturable(onCapture, _cts.Token).Forget();
    }

    public void EndDetection()
    {
        if(_cts == null) { return; }
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
    }


    private async UniTaskVoid FindCapturable(Action<Capturable> onCapture,CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested) { break; }

            Capturable capturable = _detector.DetectOne(_startPosition.position, _endPosition.position, _radius, _layer.value);
            if(capturable != null)
            {
                onCapture?.Invoke(capturable);
            }


            try { await UniTask.NextFrame(PlayerLoopTiming.Update, token); }
            catch (OperationCanceledException) { break; }
        }
    } 

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
       if(_startPosition == null
        ||_endPosition == null
        || !_showRange) { return; }

        Gizmos.DrawSphere(_startPosition.position, _radius);
        Gizmos.DrawSphere(_endPosition.position, _radius);

    }
#endif
}

namespace AnimaitonEventReceivable
{
    public interface ICaptureAnimationEventReceivable
    {
        void EnableDetection();
        void DisableDetection();
    }
}