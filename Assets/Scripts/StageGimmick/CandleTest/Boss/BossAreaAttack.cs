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
    private void Update() //�ł΂���
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
    /// �x���\���I�u�W�F�N�g�̏����ݒ�
    /// </summary>
    private void SetupWarningIndicator()
    {
        if (warningIndicator == null)
        {
            Debug.LogError("�x���\���p�̃I�u�W�F�N�g���ݒ肳��Ă��܂���B");
            return;
        }

        warningIndicatorCollider = warningIndicator.GetComponent<BoxCollider>();
        if (warningIndicatorCollider == null)
        {
            Debug.LogError("�x���\���p�I�u�W�F�N�g��BoxCollider������܂���B");
            return;
        }

        warningIndicator.SetActive(false); // ������ԂŔ�A�N�e�B�u
    }

    /// <summary>
    /// �x���\�����J�n
    /// </summary>
    public void ShowWarning()
    {
        if (warningIndicator == null) return;

        warningIndicator.SetActive(true); 
        StartCoroutine(HideWarningAfterDelay(warningDuration));
    }

    /// <summary>
    /// ���ۂ̍U������
    /// </summary>
    public void ExecuteAttack()
    {
        Vector3 boxCenter = warningIndicatorCollider.bounds.center;
        Vector3 boxHalfExtents = warningIndicatorCollider.bounds.extents;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, Quaternion.identity, 1f, targetLayer);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"�U��������: {hit.collider.gameObject.name}");
            // �_���[�W����
        }
    }

    /// <summary>
    /// �x���\�����A�N�e�B�u��
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
    /// �G�f�B�^��Ŕ͈͂����o��
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
