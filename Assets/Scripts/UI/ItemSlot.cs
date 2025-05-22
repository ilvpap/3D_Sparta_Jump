using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 UI에 나타나는 개별 아이템 슬롯을 관리하는 클래스
/// 각 슬롯은 아이템 정보, 아이콘, 수량, 선택 상태 등을 표시하고 관리함
/// </summary>
public class ItemSlot : MonoBehaviour
{
    [Header("아이템 데이터")]
    public ItemData item;       // 이 슬롯에 저장된 아이템 데이터

    [Header("UI 참조")]
    public UIInventory inventory;               // 부모 인벤토리 UI 참조
    public Button button;                       // 슬롯 클릭을 위한 버튼 컴포넌트
    public Image icon;                          // 아이템 아이콘을 표시할 이미지
    public TextMeshProUGUI quatityText;         // 아이템 수량을 표시할 텍스트
    private Outline outline;                    // 선택된 슬롯을 표시할 아웃라인 컴포넌트

    [Header("슬롯 정보")]
    public int index;           // 인벤토리에서 몇 번째 슬롯인지 나타내는 인덱스
    public bool equipped;       // 현재 장착 여부 (장착 가능한 아이템의 경우)
    public int quantity;        // 아이템 수량 데이터

    
    // 컴포넌트 초기화 - Outline 컴포넌트 참조 획득
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    
    // 슬롯이 활성화될 때 호출 - 장착 상태에 따라 아웃라인 표시
    
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// 슬롯 UI 업데이트 함수
    /// 아이템 데이터에서 필요한 정보를 가져와서 각 UI 요소에 표시
    /// </summary>
    public void Set()
    {
        // 아이템 아이콘 활성화 및 스프라이트 설정
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        // 수량 텍스트 설정 (1개보다 많을 때만 숫자 표시, 1개면 빈 문자열)
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        // 아웃라인 표시 여부 결정 (현재 선택된 아이템인지 확인)
        if (outline != null)
        {
            outline.enabled = inventory.selectedItem == this;
        }
    }

    /// <summary>
    /// 슬롯을 비우는 함수 - 아이템이 없을 때 UI를 초기화
    /// </summary>
    public void Clear()
    {
        item = null;                                    // 아이템 데이터 제거
        icon.gameObject.SetActive(false);               // 아이콘 숨김
        quatityText.text = string.Empty;                // 수량 텍스트 초기화
    }

    /// <summary>
    /// 슬롯 클릭 시 호출되는 함수 (Button 컴포넌트의 OnClick 이벤트에 연결)
    /// 인벤토리에 현재 슬롯이 선택되었음을 알림
    /// </summary>
    public void OnClickButton()
    {
        // 인벤토리의 SelectItem 호출, 현재 슬롯의 인덱스만 전달
        inventory.SelectItem(index);
    }
}