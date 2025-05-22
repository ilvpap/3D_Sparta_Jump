using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 이동, 점프, 카메라 조작, 인벤토리 열기 등 모든 기본적인 컨트롤을 담당하는 클래스
/// Unity의 새로운 Input System을 사용하여 키보드/마우스 입력을 처리함
/// 물리 기반 이동과 1인칭 카메라 시스템을 구현
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;            // 플레이어 이동 속도
    public float jumpPower = 5f;            // 점프 파워 (Rigidbody에 가해지는 힘의 크기)
    private Vector2 curMovementInput;       // 현재 입력받은 이동 입력값 (WASD 키)

    private Rigidbody _rigidbody;           // 물리 기반 이동을 위한 Rigidbody 컴포넌트

    public LayerMask groundLayerMask;       // 땅으로 인식할 레이어 마스크 (점프 가능 여부 판단용)

    [Header("카메라 조작 설정")]
    public Transform CameraContainer;       // 카메라가 들어있는 컨테이너 (상하 회전용)
    public float minXLook;                  // 카메라 상하 회전 최소 각도 (아래쪽 제한)
    public float maxXLook;                  // 카메라 상하 회전 최대 각도 (위쪽 제한)
    private float camCurXRot;               // 현재 카메라의 X축 회전값 (상하 회전)
    public float looksensitivity;           // 마우스 감도
    public Vector2 mouseDelta;              // 마우스 이동량 (프레임당 변화량)

    [Header("인터페이스 제어")]
    public bool canLook = true;             // 카메라 조작 가능 여부 (인벤토리 열림 시 false)

    // 인벤토리 열기 이벤트 - 다른 스크립트에서 구독하여 사용
    public Action inventory;

    /// <summary>
    /// 컴포넌트 초기화 - Rigidbody 참조 획득
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 게임 시작 시 초기화 - 마우스 커서 잠금
    /// </summary>
    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정하고 숨김 (1인칭 게임 스타일)
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 물리 업데이트 주기에 맞춰 이동 처리
    /// Rigidbody를 사용한 물리 기반 이동이므로 FixedUpdate에서 처리
    /// </summary>
    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 카메라 조작은 렌더링 후에 처리하여 부드러운 카메라 움직임 보장
    /// </summary>
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    /// <summary>
    /// 플레이어 이동 처리 함수
    /// 입력받은 방향키를 3D 월드 좌표계로 변환하여 Rigidbody에 적용
    /// </summary>
    void Move()
    {
        // 입력값을 플레이어의 로컬 좌표계 기준으로 이동 방향 계산
        // transform.forward: 플레이어가 바라보는 앞 방향 (Z축)
        // transform.right: 플레이어의 오른쪽 방향 (X축)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        // 이동 속도 적용
        dir *= moveSpeed;

        // Y축 속도는 기존 Rigidbody의 속도 유지 (중력과 점프 영향 보존)
        dir.y = _rigidbody.velocity.y;

        // 최종 속도를 Rigidbody에 적용
        _rigidbody.velocity = dir;
    }

    /// <summary>
    /// 1인칭 카메라 조작 처리 함수
    /// 마우스 움직임을 카메라 회전으로 변환하여 자연스러운 1인칭 시점 구현
    /// </summary>
    void CameraLook()
    {
        // 마우스 Y축 움직임으로 카메라 상하 회전 (X축 회전)
        camCurXRot += mouseDelta.y * looksensitivity;

        // 카메라 상하 회전 각도 제한 (너무 위나 아래로 돌아가지 않도록)
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        // 카메라 컨테이너의 X축 회전 적용 (음수를 사용하여 자연스러운 상하 반전)
        CameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 마우스 X축 움직임으로 플레이어 몸체 좌우 회전 (Y축 회전)
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + mouseDelta.x * looksensitivity, 0);
    }

    /// <summary>
    /// WASD 키 입력 처리 함수 (Input System 콜백)
    /// </summary>
    /// <param name="context">입력 액션 컨텍스트</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 키를 누르고 있는 동안 입력값 저장
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 키에서 손을 뗐을 때 입력값 초기화
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 마우스 움직임 입력 처리 함수 (Input System 콜백)
    /// </summary>
    /// <param name="context">입력 액션 컨텍스트</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스 움직임량 저장 (매 프레임 업데이트됨)
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 스페이스바(점프) 키 입력 처리 함수 (Input System 콜백)
    /// </summary>
    /// <param name="context">입력 액션 컨텍스트</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // 키를 누른 순간이고 땅에 서 있을 때만 점프 실행
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // Rigidbody에 위쪽 힘을 순간적으로 가하여 점프 구현
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 플레이어가 땅에 서 있는지 확인하는 함수
    /// 플레이어 발 주변 4개 지점에서 아래쪽으로 레이캐스트를 발사하여 땅과의 접촉 여부 판단
    /// </summary>
    /// <returns>땅에 서 있으면 true, 공중에 있으면 false</returns>
    bool IsGrounded()
    {
        // 플레이어 발 주변 4개 지점에서 레이캐스트 발사 (앞, 뒤, 왼쪽, 오른쪽)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),   // 앞쪽
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),  // 뒤쪽
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),     // 오른쪽
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)     // 왼쪽
        };

        // 4개 지점 중 하나라도 땅과 접촉하면 true 반환
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;   // 모든 지점에서 땅과 접촉하지 않으면 false
    }

    /// <summary>
    /// 인벤토리 키(Tab) 입력 처리 함수 (Input System 콜백)
    /// </summary>
    /// <param name="context">입력 액션 컨텍스트</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();    // 인벤토리 열기/닫기 이벤트 발생
            ToggleCursor();         // 마우스 커서 표시/숨김 토글
        }
    }

    /// <summary>
    /// 마우스 커서 표시 상태와 카메라 조작 가능 여부를 토글하는 함수
    /// 인벤토리가 열릴 때는 커서 표시, 닫힐 때는 커서 숨김
    /// </summary>
    void ToggleCursor()
    {
        // 현재 커서가 잠겨있는지 확인
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        // 커서 상태 토글 (잠금 ↔ 해제)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        // 카메라 조작 가능 여부도 같이 토글 (커서가 보일 때는 카메라 조작 불가)
        canLook = !toggle;
    }
}