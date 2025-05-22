using System;
using UnityEngine;

/// <summary>
/// 플레이어의 핵심 컴포넌트들을 통합 관리하는 메인 플레이어 클래스
/// 플레이어 관련 모든 컴포넌트들의 중앙 허브 역할을 하며
/// CharacterManager에 자신을 등록하여 전역 접근이 가능하도록 함
/// </summary>
public class Player : MonoBehaviour
{
    [Header("플레이어 핵심 컴포넌트")]
    public PlayerController controller;     // 플레이어 이동, 점프, 카메라 조작 등을 담당하는 컨트롤러
    public PlayerCondition condition;       // 플레이어의 체력, 배고픔, 스태미나 등 상태를 관리하는 컴포넌트

    [Header("아이템 시스템")]
    public ItemData itemData;               // 현재 픽업한 아이템의 데이터 (임시 저장용)
    public Action addItem;                  // 아이템 추가 시 호출되는 액션 이벤트 (인벤토리 시스템과 연동)

    [Header("월드 상호작용")]
    public Transform dropPosition;          // 아이템을 드롭할 때 생성될 위치 (플레이어 앞쪽)

    /// <summary>
    /// 오브젝트 생성 시 초기화 함수
    /// CharacterManager에 이 플레이어 인스턴스를 등록하고
    /// 필요한 컴포넌트들의 참조를 획득함
    /// </summary>
    private void Awake()
    {
        // CharacterManager의 싱글톤 인스턴스에 이 플레이어를 등록
        // 다른 스크립트에서 CharacterManager.Instance.Player로 접근 가능하게 함
        CharacterManager.Instance.Player = this;

        // 같은 게임오브젝트에 attached된 컴포넌트들의 참조 획득
        controller = GetComponent<PlayerController>();      // 플레이어 컨트롤러 컴포넌트 참조
        condition = GetComponent<PlayerCondition>();        // 플레이어 상태 관리 컴포넌트 참조
    }
}
