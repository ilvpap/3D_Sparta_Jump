using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾��� �̵�, ����, ī�޶� ����, �κ��丮 ���� �� ��� �⺻���� ��Ʈ���� ����ϴ� Ŭ����
/// Unity�� ���ο� Input System�� ����Ͽ� Ű����/���콺 �Է��� ó����
/// ���� ��� �̵��� 1��Ī ī�޶� �ý����� ����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float moveSpeed = 5f;            // �÷��̾� �̵� �ӵ�
    public float jumpPower = 5f;            // ���� �Ŀ� (Rigidbody�� �������� ���� ũ��)
    private Vector2 curMovementInput;       // ���� �Է¹��� �̵� �Է°� (WASD Ű)

    private Rigidbody _rigidbody;           // ���� ��� �̵��� ���� Rigidbody ������Ʈ

    public LayerMask groundLayerMask;       // ������ �ν��� ���̾� ����ũ (���� ���� ���� �Ǵܿ�)

    [Header("ī�޶� ���� ����")]
    public Transform CameraContainer;       // ī�޶� ����ִ� �����̳� (���� ȸ����)
    public float minXLook;                  // ī�޶� ���� ȸ�� �ּ� ���� (�Ʒ��� ����)
    public float maxXLook;                  // ī�޶� ���� ȸ�� �ִ� ���� (���� ����)
    private float camCurXRot;               // ���� ī�޶��� X�� ȸ���� (���� ȸ��)
    public float looksensitivity;           // ���콺 ����
    public Vector2 mouseDelta;              // ���콺 �̵��� (�����Ӵ� ��ȭ��)

    [Header("�������̽� ����")]
    public bool canLook = true;             // ī�޶� ���� ���� ���� (�κ��丮 ���� �� false)

    // �κ��丮 ���� �̺�Ʈ - �ٸ� ��ũ��Ʈ���� �����Ͽ� ���
    public Action inventory;

    /// <summary>
    /// ������Ʈ �ʱ�ȭ - Rigidbody ���� ȹ��
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ���� ���� �� �ʱ�ȭ - ���콺 Ŀ�� ���
    /// </summary>
    void Start()
    {
        // ���콺 Ŀ���� ȭ�� �߾ӿ� �����ϰ� ���� (1��Ī ���� ��Ÿ��)
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// ���� ������Ʈ �ֱ⿡ ���� �̵� ó��
    /// Rigidbody�� ����� ���� ��� �̵��̹Ƿ� FixedUpdate���� ó��
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// ī�޶� ������ ������ �Ŀ� ó���Ͽ� �ε巯�� ī�޶� ������ ����
    /// </summary>
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    /// <summary>
    /// �÷��̾� �̵� ó�� �Լ�
    /// �Է¹��� ����Ű�� 3D ���� ��ǥ��� ��ȯ�Ͽ� Rigidbody�� ����
    /// </summary>
    void Move()
    {
        // �Է°��� �÷��̾��� ���� ��ǥ�� �������� �̵� ���� ���
        // transform.forward: �÷��̾ �ٶ󺸴� �� ���� (Z��)
        // transform.right: �÷��̾��� ������ ���� (X��)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // �̵� �ӵ� ����
        dir *= moveSpeed;

        // Y�� �ӵ��� ���� Rigidbody�� �ӵ� ���� (�߷°� ���� ���� ����)
        dir.y = _rigidbody.velocity.y;

        // ���� �ӵ��� Rigidbody�� ����
        _rigidbody.velocity = dir;
    }

    /// <summary>
    /// 1��Ī ī�޶� ���� ó�� �Լ�
    /// ���콺 �������� ī�޶� ȸ������ ��ȯ�Ͽ� �ڿ������� 1��Ī ���� ����
    /// </summary>
    void CameraLook()
    {
        // ���콺 Y�� ���������� ī�޶� ���� ȸ�� (X�� ȸ��)
        camCurXRot += mouseDelta.y * looksensitivity;

        // ī�޶� ���� ȸ�� ���� ���� (�ʹ� ���� �Ʒ��� ���ư��� �ʵ���)
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        // ī�޶� �����̳��� X�� ȸ�� ���� (������ ����Ͽ� �ڿ������� ���� ����)
        CameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // ���콺 X�� ���������� �÷��̾� ��ü �¿� ȸ�� (Y�� ȸ��)
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + mouseDelta.x * looksensitivity, 0);
    }

    /// <summary>
    /// WASD Ű �Է� ó�� �Լ� (Input System �ݹ�)
    /// </summary>
    /// <param name="context">�Է� �׼� ���ؽ�Ʈ</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // Ű�� ������ �ִ� ���� �Է°� ����
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Ű���� ���� ���� �� �Է°� �ʱ�ȭ
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// ���콺 ������ �Է� ó�� �Լ� (Input System �ݹ�)
    /// </summary>
    /// <param name="context">�Է� �׼� ���ؽ�Ʈ</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        // ���콺 �����ӷ� ���� (�� ������ ������Ʈ��)
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// �����̽���(����) Ű �Է� ó�� �Լ� (Input System �ݹ�)
    /// </summary>
    /// <param name="context">�Է� �׼� ���ؽ�Ʈ</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // Ű�� ���� �����̰� ���� �� ���� ���� ���� ����
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // Rigidbody�� ���� ���� ���������� ���Ͽ� ���� ����
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// �÷��̾ ���� �� �ִ��� Ȯ���ϴ� �Լ�
    /// �÷��̾� �� �ֺ� 4�� �������� �Ʒ������� ����ĳ��Ʈ�� �߻��Ͽ� ������ ���� ���� �Ǵ�
    /// </summary>
    /// <returns>���� �� ������ true, ���߿� ������ false</returns>
    bool IsGrounded()
    {
        // �÷��̾� �� �ֺ� 4�� �������� ����ĳ��Ʈ �߻� (��, ��, ����, ������)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),   // ����
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),  // ����
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),     // ������
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)     // ����
        };

        // 4�� ���� �� �ϳ��� ���� �����ϸ� true ��ȯ
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;   // ��� �������� ���� �������� ������ false
    }

    /// <summary>
    /// �κ��丮 Ű(Tab) �Է� ó�� �Լ� (Input System �ݹ�)
    /// </summary>
    /// <param name="context">�Է� �׼� ���ؽ�Ʈ</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();    // �κ��丮 ����/�ݱ� �̺�Ʈ �߻�
            ToggleCursor();         // ���콺 Ŀ�� ǥ��/���� ���
        }
    }

    /// <summary>
    /// ���콺 Ŀ�� ǥ�� ���¿� ī�޶� ���� ���� ���θ� ����ϴ� �Լ�
    /// �κ��丮�� ���� ���� Ŀ�� ǥ��, ���� ���� Ŀ�� ����
    /// </summary>
    void ToggleCursor()
    {
        // ���� Ŀ���� ����ִ��� Ȯ��
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        // Ŀ�� ���� ��� (��� �� ����)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        // ī�޶� ���� ���� ���ε� ���� ��� (Ŀ���� ���� ���� ī�޶� ���� �Ұ�)
        canLook = !toggle;
    }
}