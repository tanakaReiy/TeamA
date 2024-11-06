using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class BallonController : MonoBehaviour, IInteractable
{
    [LabelText("移動距離")]
    [SerializeField] private float _moveDistance = 3.0f;
    [LabelText("移動時間")]
    [SerializeField] private float _moveDuration = 3.0f;

    private Vector3 _startPos;
    private MotionHandle _moveMotion;
    private bool _isMoving;

    // Start is called before the first frame update
    private void Start()
    {
        _startPos = this.transform.position;

        _moveMotion = LMotion
            .Create(_startPos, new Vector3(_startPos.x, _startPos.y + _moveDistance, _startPos.z), _moveDuration)
            .WithLoops(-1, LoopType.Yoyo)
            .WithEase(Ease.Linear)
            .BindToPosition(transform);

        _moveMotion.PlaybackSpeed = 0f;
    }


    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractionMessage()
    {
        return "動かす";
    }

    public void OnInteract(IInteractCallBackReceivable caller)
    {
        if (_isMoving)
            StopMotion();
        else
            StartMotion();
    }

    //Motionを再生するメソッド
    private void StartMotion()
    {
        _isMoving = true;
        _moveMotion.PlaybackSpeed = 1f;
    }

    //Motionを止めるメソッド
    private void StopMotion()
    {
        _isMoving = false;
        _moveMotion.PlaybackSpeed = 0f;
    }

    [Button]
    public void TestButton()
    {
        if (_isMoving)
            StopMotion();
        else
            StartMotion();
    }

    //移動床のコライダーに触れた時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親を移動床にする
            collision.transform.SetParent(transform);
            Debug.Log("当たった");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親をなくす
            collision.transform.SetParent(null);
            Debug.Log("離れた");
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        // ギズモで移動範囲を表示
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _moveDistance, startPosition.z);

        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPosition, this.transform.localScale);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
