using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템의 타입을 정의하는 열거형
/// 각 타입에 따라 아이템의 사용 방법과 기능이 달라짐
/// </summary>
public enum ItemType
{
    Equipable,      // 장착 가능한 아이템 (무기, 방어구 등)
    Consumable,     // 소비 가능한 아이템 (음식, 물약 등)
    Resource        // 자원 아이템 (재료, 원료 등)
}

/// <summary>
/// 소비 가능한 아이템의 효과 타입을 정의하는 열거형
/// 각 타입에 따라 플레이어의 다른 상태에 영향을 미침
/// </summary>
public enum ConsumableType
{
    Health,         // 체력에 영향을 미치는 효과 (체력 회복 등)
    Hunger          // 배고픔에 영향을 미치는 효과 (배고픔 해소 등)
}

/// <summary>
/// 소비 가능한 아이템의 개별 효과를 정의하는 클래스
/// 하나의 아이템이 여러 효과를 가질 수 있도록 배열로 사용됨
/// </summary>
[Serializable]
public class ItemDataConsumable
{
    [Header("소비 효과 설정")]
    public ConsumableType type;     // 효과의 타입 (체력, 배고픔 등)
    public int value;               // 효과의 수치 (회복량, 감소량 등)
}

/// <summary>
/// 게임 내 모든 아이템의 기본 데이터를 정의하는 ScriptableObject 클래스
/// Unity 에디터에서 아이템을 생성하고 편집할 수 있으며, 런타임에 참조하여 사용함
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("기본 아이템 정보")]
    public string displayName;      // 게임 내에서 표시될 아이템 이름
    public string description;      // 아이템에 대한 설명 텍스트
    public ItemType itemType;       // 아이템의 타입 (장착형, 소비형, 자원형)
    public Sprite icon;             // 인벤토리 UI에서 표시될 아이템 아이콘
    public GameObject dropprefab;   // 아이템을 드롭했을 때 월드에 생성될 3D 프리팹

    [Header("스택 시스템 설정")]
    public bool canStack;           // 여러 개를 하나의 슬롯에 쌓을 수 있는지 여부
    public int maxStackAmount;      // 한 슬롯에 최대로 쌓을 수 있는 개수

    [Header("소비 아이템 효과")]
    public ItemDataConsumable[] consumable;     // 소비 시 적용될 효과들의 배열
                                                // 하나의 아이템이 여러 효과를 가질 수 있음 (예: 체력과 배고픔 동시 회복)
}
