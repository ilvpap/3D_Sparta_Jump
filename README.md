# 3D_Sparta_Jump
# JumpKing 3D (Unity 프로젝트)

Unity로 구현한 1인칭 기반 점프형 미니 게임입니다.  
기본적인 이동, 점프, 아이템 습득 및 사용, 점프대 물리 작용, UI 상태 관리 등을 포함하고 있으며,  
ScriptableObject와 Coroutine을 활용한 구조 설계로 확장성을 고려해 제작하였습니다.

---

## 🎮 주요 기능

- ✅ **이동 및 점프 시스템**  
  Rigidbody 기반 이동 및 ForceMode.Impulse로 구현된 점프 기능  
  Input System 사용 (WASD + Space)

- ✅ **점프대 (JumpPad)**  
  씬에 하나의 점프대가 수동으로 배치되어 있으며, 플레이어가 점프대와 충돌할 경우 위로 강하게 튀어오릅니다.  
  점프력은 200으로 설정되어 있어 높은 구조물도 도달 가능하도록 설계되었습니다.

- ✅ **UI 상태바**  
  체력, 허기, 스태미나 수치를 실시간으로 반영하고, Image.fillAmount로 시각화

- ✅ **환경 상호작용 (Raycast 조사)**  
  전방 Raycast를 통해 IInteractable 대상 감지 후 UI에 이름/설명 표시

- ✅ **아이템 시스템**  
  ScriptableObject로 아이템을 정의하고, 사용 시 플레이어 상태에 효과 적용  
  일부 효과는 Coroutine을 통해 일정 시간 지속됩니다

---

## 📂 주요 스크립트 구성

| Script | 설명 |
|--------|------|
| `PlayerController.cs` | 기본 이동 및 점프 처리 |
| `JumpPad.cs` | 점프대 충돌 및 강한 점프 적용 (수동 배치) |
| `Interaction.cs` | 조사 대상 감지 및 UI 연동 |
| `ItemData.cs` | 아이템 데이터 구조 정의 (SO) |
| `UIInventory.cs` | 인벤토리 UI 및 아이템 조작 처리 |
| `Condition.cs` / `UICondition.cs` | 플레이어 상태 및 UI 연동 |

---

## 🧪 테스트 방법

1. `JumpTestScene`을 실행합니다.  
2. WASD 키로 이동하고, Space 키로 점프합니다.  
3. 점프대를 밟으면 강한 힘으로 캐릭터가 위로 튕겨 오릅니다.  
4. 아이템은 근접 시 `F` 키로 습득할 수 있으며, UI를 통해 사용 가능합니다.  
5. 조사 가능한 오브젝트를 바라보면 UI에 이름/설명이 표시됩니다.

---

## ⚙️ 구현 환경 및 기술 스택

- Unity 2022.x  
- C# (MonoBehaviour 기반)  
- Unity Input System  
- ScriptableObject  
- Unity UI (Canvas + TextMeshPro)

---

## 📝 개발 메모

- Debug.Log()를 적극적으로 활용해 아이템 습득/사용/충돌 로직을 시각적으로 확인했습니다.  
- UI와 상태 시스템은 구조화된 연동 구조로 구현되어 기능 확장에 유리합니다.  
- 점프대는 수동 배치지만 점프력이 충분히 높기 때문에 플레이어는 반드시 도달 가능하게 설계되었습니다.

---

## 📃 라이선스

이 프로젝트는 교육 및 포트폴리오 용도로 제작되었습니다.  
비상업적 목적의 자유로운 사용 및 수정이 가능합니다.
