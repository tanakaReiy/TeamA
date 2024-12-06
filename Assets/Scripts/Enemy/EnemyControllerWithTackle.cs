//using System;
//using UnityEngine;
//using DG.Tweening; // DoTween���g�p
//using System.Threading;

////Note:���̃R�[�h�͎R���N�����܂���
////��������s�ŃA�b�v���[�h���܂���

//[RequireComponent(typeof(Rigidbody))]
//public class EnemyControllerWithTackle : EnemyBase
//{
//    [Tooltip("�ːi���x")]
//    [SerializeField] private float rushSpeed = 10f;

//    [Tooltip("�m�b�N�o�b�N�̋���")]
//    [SerializeField] private float knockbackForce = 5f;

//    [Tooltip("�m�b�N�o�b�N�̎�������")]
//    [SerializeField] private float knockbackDuration = 0.5f;

//    [Tooltip("�ːi�𑱂��鎞��")]
//    [SerializeField] private float rushDuration = 1f;

//    private Rigidbody rb; // Rigidbody���g�p���Ĉړ�
//    private bool isRushing = false; // �^�b�N�������ǂ����̃t���O
//    private bool isKnockedBack = false; // �m�b�N�o�b�N�����ǂ����̃t���O
//    private Vector3 rushDirection; // �^�b�N���̕���

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        if (_playerTransform == null)
//        {
//            Debug.LogError("Player Transform��������܂���");
//            return;
//        }
//    }

//    private void Update()
//    {
//        // �m�b�N�o�b�N���ł���Α��̏������s��Ȃ�
//        if (isKnockedBack) return;
//        /*
//        // �ʏ펞�̍s���i�v���C���[�����m���A�ǐՂ���j
//        if (!isRushing && _enemyState == EnemyState.Move)
//        {
//            StateMove();
//        }
//        */
//    }

//    // �v���C���[�Ɍ������ă^�b�N�����J�n
//    public void StartTackle()
//    {
//        if (_playerTransform == null) return;

//        // �^�b�N���̕���������
//        rushDirection = (_playerTransform.position - transform.position).normalized;

//        // ��x�����v���C���[�̕���������
//        transform.DOLookAt(_playerTransform.position, 0.2f);

//        // �^�b�N���̃t���O��ݒ�
//        isRushing = true;
//        /*
//        // �^�b�N�����s�iDoTween��Rigidbody���g���Ĉړ��j
//        rb.DOMove(transform.position + rushDirection * rushSpeed, rushDuration)
//          .SetEase(Ease.Linear)
//          .OnComplete(() => EndTackle())
//          .SetCancellationToken(base._ctsAction.Token); // EnemyBase��CancellationToken�𗘗p
//        */
//    }

//    // �^�b�N���̏I������
//    private void EndTackle()
//    {
//        isRushing = false;
//        // ���̈ʒu�ɖ߂�
//        ReturnToOriginalPosition();
//    }

//    // �v���C���[����U�����󂯂ăm�b�N�o�b�N
//    public void TakeHit(Vector3 hitPosition)
//    {
//        if (isKnockedBack) return;

//        // �m�b�N�o�b�N�̕������v�Z�i�U�����󂯂������Ƌt�j
//        Vector3 knockbackDirection = (transform.position - hitPosition).normalized;

//        // �m�b�N�o�b�N���̃t���O��ݒ�
//        isKnockedBack = true;
//        /*
//        // �m�b�N�o�b�N������DoTween�Ŏ��s
//        rb.DOMove(transform.position + knockbackDirection * knockbackForce, knockbackDuration)
//          .SetEase(Ease.OutCubic)
//          .OnComplete(() => EndKnockback())
//          .SetCancellationToken(base._ctsAction.Token);
//        */
//    }

//    // �m�b�N�o�b�N�I������
//    private void EndKnockback()
//    {
//        isKnockedBack = false;
//        // �m�b�N�o�b�N��Ɍ��̈ʒu�ɖ߂�
//        ReturnToOriginalPosition();
//    }

//    // ���̈ʒu�ɖ߂�
//    private void ReturnToOriginalPosition()
//    {
//        /*
//        rb.DOMove(originalPosition, 1f)
//          .SetEase(Ease.OutCubic)
//          .SetCancellationToken(base._ctsAction.Token);

//        // ��]�����ɖ߂�
//        transform.DORotateQuaternion(originalRotation, 1f)
//                 .SetEase(Ease.OutCubic)
//                 .SetCancellationToken(base._ctsAction.Token);
//        */
//    }

//    /*
//    // ���m�͈͂⎋��p�̃M�Y����\��
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
