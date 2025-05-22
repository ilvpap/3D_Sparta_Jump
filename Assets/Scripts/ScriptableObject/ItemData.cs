using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� Ÿ���� �����ϴ� ������
/// �� Ÿ�Կ� ���� �������� ��� ����� ����� �޶���
/// </summary>
public enum ItemType
{
    Equipable,      // ���� ������ ������ (����, �� ��)
    Consumable,     // �Һ� ������ ������ (����, ���� ��)
    Resource        // �ڿ� ������ (���, ���� ��)
}

/// <summary>
/// �Һ� ������ �������� ȿ�� Ÿ���� �����ϴ� ������
/// �� Ÿ�Կ� ���� �÷��̾��� �ٸ� ���¿� ������ ��ħ
/// </summary>
public enum ConsumableType
{
    Health,         // ü�¿� ������ ��ġ�� ȿ�� (ü�� ȸ�� ��)
    Hunger          // ����Ŀ� ������ ��ġ�� ȿ�� (����� �ؼ� ��)
}

/// <summary>
/// �Һ� ������ �������� ���� ȿ���� �����ϴ� Ŭ����
/// �ϳ��� �������� ���� ȿ���� ���� �� �ֵ��� �迭�� ����
/// </summary>
[Serializable]
public class ItemDataConsumable
{
    [Header("�Һ� ȿ�� ����")]
    public ConsumableType type;     // ȿ���� Ÿ�� (ü��, ����� ��)
    public int value;               // ȿ���� ��ġ (ȸ����, ���ҷ� ��)
}

/// <summary>
/// ���� �� ��� �������� �⺻ �����͸� �����ϴ� ScriptableObject Ŭ����
/// Unity �����Ϳ��� �������� �����ϰ� ������ �� ������, ��Ÿ�ӿ� �����Ͽ� �����
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("�⺻ ������ ����")]
    public string displayName;      // ���� ������ ǥ�õ� ������ �̸�
    public string description;      // �����ۿ� ���� ���� �ؽ�Ʈ
    public ItemType itemType;       // �������� Ÿ�� (������, �Һ���, �ڿ���)
    public Sprite icon;             // �κ��丮 UI���� ǥ�õ� ������ ������
    public GameObject dropprefab;   // �������� ������� �� ���忡 ������ 3D ������

    [Header("���� �ý��� ����")]
    public bool canStack;           // ���� ���� �ϳ��� ���Կ� ���� �� �ִ��� ����
    public int maxStackAmount;      // �� ���Կ� �ִ�� ���� �� �ִ� ����

    [Header("�Һ� ������ ȿ��")]
    public ItemDataConsumable[] consumable;     // �Һ� �� ����� ȿ������ �迭
                                                // �ϳ��� �������� ���� ȿ���� ���� �� ���� (��: ü�°� ����� ���� ȸ��)
}
