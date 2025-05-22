using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI 전체를 관리하는 메인 클래스
/// 아이템 슬롯 관리, 아이템 추가/제거, 사용/장착/드롭 기능, 선택된 아이템 정보 표시 등을 담당
/// </summary>
public class UIInventory : MonoBehaviour
{
    [Header("인벤토리 슬롯")]
    public ItemSlot[] slots;                    // 모든 아이템 슬롯 배열

    [Header("인벤토리 창 UI")]
    public GameObject inventoryWindow;          // 인벤토리 전체 창 오브젝트
    public Transform slotPanel;                 // 슬롯들이 포함된 패널
    public Transform dropPosition;              // 아이템 드롭 시 생성될 위치

    [Header("선택된 아이템 정보 UI")]
    public ItemSlot selectedItem;               // 현재 선택된 아이템 슬롯 참조
    private int selectedItemIndex;              // 선택된 아이템의 슬롯 인덱스
    public TextMeshProUGUI selectedItemName;        // 선택된 아이템 이름 표시 텍스트
    public TextMeshProUGUI selectedItemDescription; // 선택된 아이템 설명 표시 텍스트
    public TextMeshProUGUI selectedItemStatName;    // 선택된 아이템 스탯 이름 표시 텍스트
    public TextMeshProUGUI selectedItemStatValue;   // 선택된 아이템 스탯 값 표시 텍스트
    public GameObject useButton;                // 아이템 사용 버튼
    public GameObject equipButton;              // 아이템 장착 버튼
    public GameObject unEquipButton;            // 아이템 해제 버튼
    public GameObject dropButton;               // 아이템 드롭 버튼

    private int curEquipIndex;                  // 현재 장착된 아이템의 인덱스

    private PlayerController controller;        // 플레이어 컨트롤러 참조
    private PlayerCondition condition;          // 플레이어 상태 참조

    /// <summary>
    /// 초기화 - 참조 설정, 이벤트 연결, 슬롯 초기화
    /// </summary>
    void Start()
    {
        // 플레이어 관련 컴포넌트 참조 획득
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 입력 이벤트 연결
        controller.inventory += Toggle;                             // 인벤토리 키 입력 시 Toggle 함수 호출
        CharacterManager.Instance.Player.addItem += AddItem;       // 아이템 픽업 시 AddItem 함수 호출

        // 인벤토리 UI 초기화
        inventoryWindow.SetActive(false);                          // 게임 시작 시 인벤토리 창 숨김
        slots = new ItemSlot[slotPanel.childCount];                // 슬롯 배열 크기를 패널의 자식 수만큼 설정

        // 모든 슬롯 초기화 및 설정
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();  // 각 슬롯 컴포넌트 참조
            slots[i].index = i;                                         // 슬롯 인덱스 설정
            slots[i].inventory = this;                                  // 슬롯이 이 인벤토리를 참조하도록 설정
            slots[i].Clear();                                           // 슬롯 초기화
        }

        ClearSelectedItemWindow();                                 // 선택된 아이템 정보창 초기화
    }

    /// <summary>
    /// 선택된 아이템 정보 표시 창을 초기화하는 함수
    /// 아이템 선택이 해제되거나 초기화할 때 호출
    /// </summary>
    void ClearSelectedItemWindow()
    {
        selectedItem = null;                            // 선택된 아이템 참조 해제

        // 모든 텍스트 필드 초기화
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // 모든 버튼 비활성화
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 창 열기/닫기 토글 함수
    /// 플레이어가 인벤토리 키를 누를 때 호출됨
    /// </summary>
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);           // 열려있으면 닫기
        }
        else
        {
            inventoryWindow.SetActive(true);            // 닫혀있으면 열기
        }
    }

    /// <summary>
    /// 인벤토리 창이 현재 열려있는지 확인하는 함수
    /// </summary>
    /// <returns>인벤토리 창 활성화 상태</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// 새로운 아이템을 인벤토리에 추가하는 함수
    /// 플레이어가 아이템을 픽업할 때 자동으로 호출됨
    /// </summary>
    public void AddItem()
    {
        // CharacterManager에서 플레이어가 픽업한 아이템 데이터 가져오기
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 아이템 데이터 유효성 검사
        if (data == null)
        {
            Debug.LogWarning("[UIInventory] 아이템 데이터가 null입니다!");
            return;
        }

        Debug.Log($"[UIInventory] {data.displayName} 추가 시도");

        // 스택 가능한 아이템인 경우 기존 스택에 추가 시도
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;                                    // 기존 스택 수량 증가
                UpdateUI();                                         // UI 업데이트
                CharacterManager.Instance.Player.itemData = null;  // 픽업한 아이템 데이터 초기화
                return;
            }
        }

        // 빈 슬롯 찾기
        ItemSlot emptySlot = GetEmptySlot();

        // 빈 슬롯이 있다면 새로운 아이템 추가
        if (emptySlot != null)
        {
            emptySlot.item = data;                              // 아이템 데이터 설정
            emptySlot.quantity = 1;                             // 수량 1로 설정
            UpdateUI();                                         // UI 업데이트
            CharacterManager.Instance.Player.itemData = null;  // 픽업한 아이템 데이터 초기화
            return;
        }

        // 빈 슬롯이 없을 때 아이템을 땅에 드롭
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    /// <summary>
    /// 모든 슬롯의 UI를 최신 상태로 업데이트하는 함수
    /// 아이템 추가, 제거, 사용 후 호출되어 화면을 갱신함
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯에 아이템 정보가 있다면 UI 표시, 없다면 초기화
            if (slots[i].item != null)
            {
                slots[i].Set();         // 아이템 정보로 UI 설정
            }
            else
            {
                slots[i].Clear();       // UI 초기화
            }
        }
    }

    /// <summary>
    /// 스택 가능한 아이템의 기존 스택을 찾는 함수
    /// 같은 아이템이면서 최대 스택 수량에 도달하지 않은 슬롯을 반환
    /// </summary>
    /// <param name="data">찾을 아이템 데이터</param>
    /// <returns>스택 가능한 슬롯 또는 null</returns>
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 비어있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>빈 슬롯 또는 null</returns>
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 아이템을 월드에 드롭하는 함수
    /// 인벤토리가 가득 찰 때나 플레이어가 의도적으로 드롭할 때 사용
    /// </summary>
    /// <param name="data">드롭할 아이템 데이터</param>
    public void ThrowItem(ItemData data)
    {
        // 아이템의 드롭 프리팹을 플레이어 드롭 위치에 생성
        // 랜덤한 회전값을 적용하여 자연스럽게 떨어뜨림
        Instantiate(data.dropprefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 선택하고 정보창에 표시하는 함수
    /// ItemSlot에서 호출되어 선택된 아이템의 상세 정보를 표시함
    /// </summary>
    /// <param name="index">선택할 슬롯의 인덱스</param>
    public void SelectItem(int index)
    {
        // 선택한 슬롯에 아이템이 없으면 함수 종료
        if (slots[index].item == null) return;

        // 기존 선택된 슬롯의 아웃라인 비활성화
        if (selectedItem != null)
        {
            selectedItem.equipped = false;
            selectedItem.GetComponent<Outline>().enabled = false;
        }

        // 새로운 슬롯 선택 및 인덱스 저장
        selectedItem = slots[index];
        selectedItemIndex = index;

        // 새로 선택된 슬롯의 아웃라인 활성화
        selectedItem.equipped = true;
        selectedItem.GetComponent<Outline>().enabled = true;

        // 선택된 아이템의 기본 정보 표시
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        // 스탯 정보 초기화
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // 소비 가능한 아이템의 스탯 정보 표시
        for (int i = 0; i < selectedItem.item.consumable.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[i].value.ToString() + "\n";
        }

        // 아이템 타입에 따라 버튼 활성화/비활성화
        useButton.SetActive(selectedItem.item.itemType == ItemType.Consumable);                                  // 소비 아이템만 사용 버튼 표시
        equipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && !selectedItem.equipped);      // 장착 가능하고 미장착 상태일 때 장착 버튼 표시
        unEquipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && selectedItem.equipped);     // 장착 가능하고 장착 상태일 때 해제 버튼 표시
        dropButton.SetActive(true);                                                                             // 드롭 버튼은 항상 표시
    }

    /// <summary>
    /// 선택된 아이템을 사용하는 함수 (소비 아이템 전용)
    /// 사용 버튼 클릭 시 호출됨
    /// </summary>
    public void OnUseButton()
    {
        // 소비 가능한 아이템인지 확인
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            // 아이템의 모든 소비 효과 적용
            for (int i = 0; i < selectedItem.item.consumable.Length; i++)
            {
                switch (selectedItem.item.consumable[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumable[i].value);     // 체력 회복
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumable[i].value);      // 배고픔 해소
                        break;
                }
            }
            RemoveSelctedItem();    // 사용 후 아이템 제거
        }
    }

    /// <summary>
    /// 선택된 아이템을 드롭하는 함수
    /// 드롭 버튼 클릭 시 호출됨
    /// </summary>
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);   // 아이템을 월드에 드롭
        RemoveSelctedItem();            // 인벤토리에서 아이템 제거
    }

    /// <summary>
    /// 현재 선택된 아이템을 인벤토리에서 제거하는 함수
    /// 아이템 사용이나 드롭 후 호출되어 수량을 감소시키거나 완전히 제거함
    /// </summary>
    void RemoveSelctedItem()
    {
        slots[selectedItemIndex].quantity--;    // 수량 1 감소

        // 수량이 0 이하가 되면 슬롯을 완전히 비우고 선택 해제
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();          // 선택된 아이템 정보창 초기화
        }

        UpdateUI();                             // UI 업데이트
    }

    /// <summary>
    /// 장착 해제 함수 (현재 미구현)
    /// 장착된 아이템을 해제할 때 사용할 예정
    /// </summary>
    /// <param name="index">해제할 아이템의 슬롯 인덱스</param>
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        // 추가적인 장착 해제 로직은 여기에 구현 예정
    }

    /// <summary>
    /// 특정 아이템의 보유 여부와 수량을 확인하는 함수 (현재 미구현)
    /// 다른 시스템에서 플레이어가 특정 아이템을 가지고 있는지 확인할 때 사용할 예정
    /// </summary>
    /// <param name="item">확인할 아이템</param>
    /// <param name="quantity">필요한 수량</param>
    /// <returns>보유 여부</returns>
    public bool HasItem(ItemData item, int quantity)
    {
        return false;   // 현재는 항상 false 반환 (미구현)
    }
}
