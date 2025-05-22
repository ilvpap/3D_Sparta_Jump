using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어가 게임 내 상호작용 가능한 객체들과 상호작용할 수 있게 해주는 클래스
/// 화면 중앙에서 레이캐스트를 발사하여 상호작용 가능한 객체를 감지하고
/// UI 프롬프트 표시 및 상호작용 입력을 처리함
/// </summary>
public class Interaction : MonoBehaviour
{
    [Header("상호작용 감지 설정")]
    public float CheckRate = 0.05f;         // 상호작용 대상 확인 주기 (초 단위, 성능 최적화용)
    private float lastCheckTime;            // 마지막으로 확인한 시간 저장
    public float maxCheckDistance;          // 상호작용 가능한 최대 거리
    public LayerMask layerMask;             // 상호작용 대상 레이어 마스크 (특정 레이어만 감지)

    [Header("현재 상호작용 대상")]
    public GameObject curInteractGameObject;    // 현재 상호작용 가능한 게임오브젝트
    private IInteractable curInteractable;      // 현재 상호작용 가능한 객체의 인터페이스

    [Header("UI")]
    public TextMeshProUGUI promptText;      // 상호작용 프롬프트를 표시할 텍스트 UI
    private Camera camera;                  // 레이캐스트 발사를 위한 메인 카메라 참조

    /// <summary>
    /// 초기화 - 메인 카메라 참조 획득
    /// </summary>
    void Start()
    {
        camera = Camera.main;
    }

    /// <summary>
    /// 매 프레임마다 상호작용 대상 확인
    /// CheckRate 주기로만 실행하여 성능 최적화
    /// </summary>
    void Update()
    {
        // CheckRate 시간이 지났을 때만 상호작용 대상 확인 (성능 최적화)
        if (Time.time - lastCheckTime > CheckRate)
        {
            lastCheckTime = Time.time;

            // 화면 중앙(크로스헤어 위치)에서 레이 발사
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // 레이캐스트로 상호작용 가능한 객체 감지
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 새로운 상호작용 대상을 발견한 경우
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;                        // 새 대상으로 설정
                    curInteractable = hit.collider.GetComponent<IInteractable>();           // 상호작용 인터페이스 획득
                    SetPromptText();                                                        // 프롬프트 텍스트 설정
                }
            }
            else
            {
                // 상호작용 가능한 객체가 없는 경우 초기화
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);     // 프롬프트 텍스트 숨김
            }
        }
    }

    /// <summary>
    /// 상호작용 프롬프트 텍스트를 설정하고 표시하는 함수
    /// 현재 감지된 상호작용 가능한 객체의 이름을 표시함
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);                          // 프롬프트 텍스트 활성화
        promptText.text = curInteractable.GetInteractableName();        // 상호작용 객체의 이름 설정
    }

    /// <summary>
    /// 플레이어가 상호작용 키를 눌렀을 때 호출되는 함수
    /// Input System의 콜백으로 연결되어 사용됨
    /// </summary>
    /// <param name="context">입력 액션 컨텍스트</param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // 키를 누른 순간이고, 상호작용 가능한 객체가 있을 때만 실행
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();                   // 상호작용 실행

            // 상호작용 완료 후 상태 초기화
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);         // 프롬프트 텍스트 숨김
        }
    }
}
