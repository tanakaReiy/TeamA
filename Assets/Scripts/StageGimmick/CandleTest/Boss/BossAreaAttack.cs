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
    private void Update() //�ł΂���
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
    /// �x���\���I�u�W�F�N�g�̏����ݒ�
    /// </summary>
    private void SetupWarningIndicator()
    {
        if (_warningIndicator == null)
        {
            Debug.LogError("�x���\���p�̃I�u�W�F�N�g���ݒ肳��Ă��܂���B");
            return;
        }

        _warningIndicatorCollider = _warningIndicator.GetComponent<BoxCollider>();
        if (_warningIndicatorCollider == null)
        {
            Debug.LogError("�x���\���p�I�u�W�F�N�g��BoxCollider������܂���B");
            return;
        }

        _warningIndicator.SetActive(false); // ������ԂŔ�A�N�e�B�u
    }

    /// <summary>
    /// �x���\�����J�n
    /// </summary>
    public void ShowWarning()
    {
        if (_warningIndicator == null) return;

        _warningIndicator.SetActive(true);
        StartCoroutine(HideWarningAfterDelay(_warningDuration));
    }

    /// <summary>
    /// ���ۂ̍U������
    /// </summary>
    public void ExecuteAttack(float damage)
    {
        Vector3 boxCenter = _warningIndicatorCollider.bounds.center;
        Vector3 boxHalfExtents = _warningIndicatorCollider.bounds.extents;
        Quaternion boxRotation = transform.rotation;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, boxRotation, 1f, _targetLayer);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"�U���͈͓��Ƀv���C���[�����m");
            // �_���[�W����
            if (hit.collider.TryGetComponent<PlayerDamageReceiver>(out PlayerDamageReceiver damageReceiver))
            {
                damageReceiver.ApplyDamage(damage);
                Debug.Log($"{hit.collider.name}��{damage}�_���[�W�^����");
            }
        }
    }

    /// <summary>
    /// �x���\�����A�N�e�B�u��
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
    /// �G�f�B�^��Ŕ͈͂����o��
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
