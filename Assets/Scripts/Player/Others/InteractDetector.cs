using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Alchemy.Inspector;
public class InteractDetector : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private bool _showRange = false;
    [SerializeField] private LayerMask _detectLayer;
    [SerializeField] private Vector3 _offset;

    //Event
    public IReadOnlyReactiveProperty<IInteractable> CurrentInteractable => _currentInteractable;

    //public Property
    public bool IsDetectionEnabled
    {
        get => _enableDetection;
        set{
            _enableDetection = value;
            if (!value)
            {
                _currentInteractable.Value = null;            
            }
        }
    }

    //private
    private const float DETECTCALCINTERVAL = 0.1f;

    private bool _enableDetection = true;
    private ReactiveProperty<IInteractable> _currentInteractable = new(null);
    private IInteractCallBackReceivable _callBackReceivable;

    private void Start()
    {
        if (!gameObject.TryGetComponent(out _callBackReceivable))
        {
            throw new System.Exception($"{gameObject.name}は{nameof(IInteractCallBackReceivable)}を実装している必要があります");
        }

        StartCoroutine(Detect());
    }

    /// <summary>
    /// 範囲内にあるInteractableに対してインタラクトします
    /// </summary>
    /// <returns>インタラクトできたかどうか</returns>
    public bool Interact()
    {
        if (_currentInteractable.Value != null)
        {
            _currentInteractable.Value.OnInteract(_callBackReceivable);
            return true;
        }
        else
        {
            return false;
        }
        
    }

    //一定間隔でInteractableを検知する
    private IEnumerator Detect()
    {
        
        while (true)
        {
            if (!IsDetectionEnabled) { goto There; }

            //検知
            RaycastHit[] hits = Physics.SphereCastAll(
                new Ray { origin = transform.TransformPoint(_offset), direction = transform.forward },_radius,0.1f,_detectLayer.value);

            
            IInteractable interactable = null;
            foreach (var hit in hits)
            {
                //Detect Interactable
                if(hit.collider.gameObject.TryGetComponent(out  interactable))
                {
                    //インタラクトできないInteractableの場合はスキップ
                    if (!interactable.CanInteract()) { continue; }
                    else { break; }
                }
            }

            //発見したInteractable or nullが現在と異なるときのみ値を更新
            if(interactable != _currentInteractable.Value)
            {
                _currentInteractable.Value = interactable;
            }

        There:
            yield return new WaitForSeconds(DETECTCALCINTERVAL);
        }
    }


    private void OnDrawGizmosSelected()
    {

        if(!_showRange) { return; }

        Gizmos.DrawSphere(transform.TransformPoint(_offset), _radius);

    }

}

