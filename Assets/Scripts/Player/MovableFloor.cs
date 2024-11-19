using LitMotion;
using LitMotion.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovableFloor : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _size;

    private CharacterMovement _cache = null;
    void Start()
    {
        
        LitMotion.LMotion.Create(transform.position + new Vector3(0, -10, 0), transform.position + new Vector3(0, 10, 0), 5f)
            .WithLoops(-1,LoopType.Yoyo)
            .BindToLocalPosition(transform);
    }

    private void Update()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.TransformVector(_offset), _size / 2f);
        bool found = false;
        foreach(var collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out CharacterMovement characterMovement))
            {
                found = true;
                if(characterMovement == _cache) { return; }
                characterMovement.SetFollowTarget(transform);
                _cache = characterMovement;
            }
        }
        if (!found && _cache != null)
        {
            _cache.SetFollowTarget(null);
            _cache = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + transform.TransformVector(_offset), _size);
    }
}
