using UnityEngine;

// 플레이어의 모든 상태 조건들을 하나로 묶어서 관리하는 UI 컨트롤러 클래스
// 체력, 배고픔, 스태미나 등 개별 Condition들을 그룹화하여 관리
// PlayerCondition에서 이 클래스를 참조하여 각 상태에 접근함
public class UICondition : MonoBehaviour
{
    [Header("플레이어 상태 UI")]
    public Condition health;    // 체력 상태 UI 및 로직 관리
    public Condition hunger;    // 배고픔 상태 UI 및 로직 관리  
    public Condition stamina;   // 스태미나 상태 UI 및 로직 관리

    // 초기화 - PlayerCondition에 이 UICondition 인스턴스를 등록
    // PlayerCondition이 각 상태에 접근할 수 있도록 연결점 역할
    private void Start()
    {
        // CharacterManager를 통해 플레이어의 condition 컴포넌트에 이 UI를 연결
        // 이렇게 하면 PlayerCondition에서 health, hunger, stamina에 직접 접근 가능
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}