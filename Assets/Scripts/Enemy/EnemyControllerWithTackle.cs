//using System;
//using UnityEngine;
//using DG.Tweening; // DoTweenを使用
//using System.Threading;

////Note:このコードは山口君が作りました
////岡村が代行でアップロードしました

//[RequireComponent(typeof(Rigidbody))]
//public class EnemyControllerWithTackle : EnemyBase
//{
//    [Tooltip("突進速度")]
//    [SerializeField] private float rushSpeed = 10f;

//    [Tooltip("ノックバックの強さ")]
//    [SerializeField] private float knockbackForce = 5f;

//    [Tooltip("ノックバックの持続時間")]
//    [SerializeField] private float knockbackDuration = 0.5f;

//    [Tooltip("突進を続ける時間")]
//    [SerializeField] private float rushDuration = 1f;

//    private Rigidbody rb; // Rigidbodyを使用して移動
//    private bool isRushing = false; // タックル中かどうかのフラグ
//    private bool isKnockedBack = false; // ノックバック中かどうかのフラグ
//    private Vector3 rushDirection; // タックルの方向

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        if (_playerTransform == null)
//        {
//            Debug.LogError("Player Transformが見つかりません");
//            return;
//        }
//    }

//    private void Update()
//    {
//        // ノックバック中であれば他の処理を行わない
//        if (isKnockedBack) return;
//        /*
//        // 通常時の行動（プレイヤーを検知し、追跡する）
//        if (!isRushing && _enemyState == EnemyState.Move)
//        {
//            StateMove();
//        }
//        */
//    }

//    // プレイヤーに向かってタックルを開始
//    public void StartTackle()
//    {
//        if (_playerTransform == null) return;

//        // タックルの方向を決定
//        rushDirection = (_playerTransform.position - transform.position).normalized;

//        // 一度だけプレイヤーの方向を向く
//        transform.DOLookAt(_playerTransform.position, 0.2f);

//        // タックルのフラグを設定
//        isRushing = true;
//        /*
//        // タックル実行（DoTweenでRigidbodyを使って移動）
//        rb.DOMove(transform.position + rushDirection * rushSpeed, rushDuration)
//          .SetEase(Ease.Linear)
//          .OnComplete(() => EndTackle())
//          .SetCancellationToken(base._ctsAction.Token); // EnemyBaseのCancellationTokenを利用
//        */
//    }

//    // タックルの終了処理
//    private void EndTackle()
//    {
//        isRushing = false;
//        // 元の位置に戻る
//        ReturnToOriginalPosition();
//    }

//    // プレイヤーから攻撃を受けてノックバック
//    public void TakeHit(Vector3 hitPosition)
//    {
//        if (isKnockedBack) return;

//        // ノックバックの方向を計算（攻撃を受けた方向と逆）
//        Vector3 knockbackDirection = (transform.position - hitPosition).normalized;

//        // ノックバック中のフラグを設定
//        isKnockedBack = true;
//        /*
//        // ノックバック処理をDoTweenで実行
//        rb.DOMove(transform.position + knockbackDirection * knockbackForce, knockbackDuration)
//          .SetEase(Ease.OutCubic)
//          .OnComplete(() => EndKnockback())
//          .SetCancellationToken(base._ctsAction.Token);
//        */
//    }

//    // ノックバック終了処理
//    private void EndKnockback()
//    {
//        isKnockedBack = false;
//        // ノックバック後に元の位置に戻る
//        ReturnToOriginalPosition();
//    }

//    // 元の位置に戻る
//    private void ReturnToOriginalPosition()
//    {
//        /*
//        rb.DOMove(originalPosition, 1f)
//          .SetEase(Ease.OutCubic)
//          .SetCancellationToken(base._ctsAction.Token);

//        // 回転も元に戻す
//        transform.DORotateQuaternion(originalRotation, 1f)
//                 .SetEase(Ease.OutCubic)
//                 .SetCancellationToken(base._ctsAction.Token);
//        */
//    }

//    /*
//    // 検知範囲や視野角のギズモを表示
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, _searchablePlayerDistance);

//        Vector3 leftDirection = Quaternion.Euler(0, -_fieldOfViewHalf, 0) * transform.forward;
//        Gizmos.color = Color.blue;
//        Gizmos.DrawLine(transform.position, transform.position + leftDirection * _searchablePlayerDistance);

//        Vector3 rightDirection = Quaternion.Euler(0, _fieldOfViewHalf, 0) * transform.forward;
//        Gizmos.DrawLine(transform.position, transform.position + rightDirection * _searchablePlayerDistance);
//    }
//    */
//}
