using System.Collections;
using UnityEngine;

public class BossAreaAttack : MonoBehaviour
{
    [SerializeField] private GameObject _warningIndicator;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _warningDuration = 3f;

    private BoxCollider _warningIndicatorCollider;

    private void Start()
    {
        SetupWarningIndicator();
    }
    private void Update() //でばっく
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowWarning();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ExecuteAttack(1);
        }
    }
    /// <summary>
    /// 警告表示オブジェクトの初期設定
    /// </summary>
    private void SetupWarningIndicator()
    {
        if (_warningIndicator == null)
        {
            Debug.LogError("警告表示用のオブジェクトが設定されていません。");
            return;
        }

        _warningIndicatorCollider = _warningIndicator.GetComponent<BoxCollider>();
        if (_warningIndicatorCollider == null)
        {
            Debug.LogError("警告表示用オブジェクトにBoxColliderがありません。");
            return;
        }

        _warningIndicator.SetActive(false); // 初期状態で非アクティブ
    }

    /// <summary>
    /// 警告表示を開始
    /// </summary>
    public void ShowWarning()
    {
        if (_warningIndicator == null) return;

        _warningIndicator.SetActive(true);
        StartCoroutine(HideWarningAfterDelay(_warningDuration));
    }

    /// <summary>
    /// 実際の攻撃処理
    /// </summary>
    public void ExecuteAttack(float damage)
    {
        Vector3 boxCenter = _warningIndicatorCollider.bounds.center;
        Vector3 boxHalfExtents = _warningIndicatorCollider.bounds.extents;
        Quaternion boxRotation = transform.rotation;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, boxRotation, 1f, _targetLayer);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"攻撃範囲内にプレイヤーを検知");
            // ダメージ処理
            if (hit.collider.TryGetComponent<PlayerDamageReceiver>(out PlayerDamageReceiver damageReceiver))
            {
                damageReceiver.ApplyDamage(damage);
                Debug.Log($"{hit.collider.name}に{damage}ダメージ与えた");
            }
        }
    }

    /// <summary>
    /// 警告表示を非アクティブ化
    /// </summary>
    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_warningIndicator != null)
        {
            _warningIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// エディタ上で範囲を視覚化
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        var cube = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(this.transform.localPosition, this.transform.localRotation, this.transform.localScale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_warningIndicator.transform.localPosition, _warningIndicator.transform.localScale);
        Gizmos.matrix = cube;
    }
}
