using System.Collections;
using UnityEngine;

public class BossAreaAttack : MonoBehaviour
{
    [SerializeField] private GameObject warningIndicator;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float warningDuration = 3f;

    private BoxCollider warningIndicatorCollider;

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
            ExecuteAttack();
        }
    }
    /// <summary>
    /// 警告表示オブジェクトの初期設定
    /// </summary>
    private void SetupWarningIndicator()
    {
        if (warningIndicator == null)
        {
            Debug.LogError("警告表示用のオブジェクトが設定されていません。");
            return;
        }

        warningIndicatorCollider = warningIndicator.GetComponent<BoxCollider>();
        if (warningIndicatorCollider == null)
        {
            Debug.LogError("警告表示用オブジェクトにBoxColliderがありません。");
            return;
        }

        warningIndicator.SetActive(false); // 初期状態で非アクティブ
    }

    /// <summary>
    /// 警告表示を開始
    /// </summary>
    public void ShowWarning()
    {
        if (warningIndicator == null) return;

        warningIndicator.SetActive(true); 
        StartCoroutine(HideWarningAfterDelay(warningDuration));
    }

    /// <summary>
    /// 実際の攻撃処理
    /// </summary>
    public void ExecuteAttack()
    {
        Vector3 boxCenter = warningIndicatorCollider.bounds.center;
        Vector3 boxHalfExtents = warningIndicatorCollider.bounds.extents;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, Quaternion.identity, 1f, targetLayer);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"攻撃が命中: {hit.collider.gameObject.name}");
            // ダメージ処理
        }
    }

    /// <summary>
    /// 警告表示を非アクティブ化
    /// </summary>
    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (warningIndicator != null)
        {
            warningIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// エディタ上で範囲を視覚化
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (warningIndicator != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(warningIndicator.GetComponent<BoxCollider>().bounds.center, warningIndicator.GetComponent<BoxCollider>().bounds.size);
        }
    }
}
