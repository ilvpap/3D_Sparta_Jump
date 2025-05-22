using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// �κ��丮 UI ��ü�� �����ϴ� ���� Ŭ����
/// ������ ���� ����, ������ �߰�/����, ���/����/��� ���, ���õ� ������ ���� ǥ�� ���� ���
/// </summary>
public class UIInventory : MonoBehaviour
{
    [Header("�κ��丮 ����")]
    public ItemSlot[] slots;                    // ��� ������ ���� �迭

    [Header("�κ��丮 â UI")]
    public GameObject inventoryWindow;          // �κ��丮 ��ü â ������Ʈ
    public Transform slotPanel;                 // ���Ե��� ���Ե� �г�
    public Transform dropPosition;              // ������ ��� �� ������ ��ġ

    [Header("���õ� ������ ���� UI")]
    public ItemSlot selectedItem;               // ���� ���õ� ������ ���� ����
    private int selectedItemIndex;              // ���õ� �������� ���� �ε���
    public TextMeshProUGUI selectedItemName;        // ���õ� ������ �̸� ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI selectedItemDescription; // ���õ� ������ ���� ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI selectedItemStatName;    // ���õ� ������ ���� �̸� ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI selectedItemStatValue;   // ���õ� ������ ���� �� ǥ�� �ؽ�Ʈ
    public GameObject useButton;                // ������ ��� ��ư
    public GameObject equipButton;              // ������ ���� ��ư
    public GameObject unEquipButton;            // ������ ���� ��ư
    public GameObject dropButton;               // ������ ��� ��ư

    private int curEquipIndex;                  // ���� ������ �������� �ε���

    private PlayerController controller;        // �÷��̾� ��Ʈ�ѷ� ����
    private PlayerCondition condition;          // �÷��̾� ���� ����

    /// <summary>
    /// �ʱ�ȭ - ���� ����, �̺�Ʈ ����, ���� �ʱ�ȭ
    /// </summary>
    void Start()
    {
        // �÷��̾� ���� ������Ʈ ���� ȹ��
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // �Է� �̺�Ʈ ����
        controller.inventory += Toggle;                             // �κ��丮 Ű �Է� �� Toggle �Լ� ȣ��
        CharacterManager.Instance.Player.addItem += AddItem;       // ������ �Ⱦ� �� AddItem �Լ� ȣ��

        // �κ��丮 UI �ʱ�ȭ
        inventoryWindow.SetActive(false);                          // ���� ���� �� �κ��丮 â ����
        slots = new ItemSlot[slotPanel.childCount];                // ���� �迭 ũ�⸦ �г��� �ڽ� ����ŭ ����

        // ��� ���� �ʱ�ȭ �� ����
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();  // �� ���� ������Ʈ ����
            slots[i].index = i;                                         // ���� �ε��� ����
            slots[i].inventory = this;                                  // ������ �� �κ��丮�� �����ϵ��� ����
            slots[i].Clear();                                           // ���� �ʱ�ȭ
        }

        ClearSelectedItemWindow();                                 // ���õ� ������ ����â �ʱ�ȭ
    }

    /// <summary>
    /// ���õ� ������ ���� ǥ�� â�� �ʱ�ȭ�ϴ� �Լ�
    /// ������ ������ �����ǰų� �ʱ�ȭ�� �� ȣ��
    /// </summary>
    void ClearSelectedItemWindow()
    {
        selectedItem = null;                            // ���õ� ������ ���� ����

        // ��� �ؽ�Ʈ �ʵ� �ʱ�ȭ
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // ��� ��ư ��Ȱ��ȭ
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    /// <summary>
    /// �κ��丮 â ����/�ݱ� ��� �Լ�
    /// �÷��̾ �κ��丮 Ű�� ���� �� ȣ���
    /// </summary>
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);           // ���������� �ݱ�
        }
        else
        {
            inventoryWindow.SetActive(true);            // ���������� ����
        }
    }

    /// <summary>
    /// �κ��丮 â�� ���� �����ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>�κ��丮 â Ȱ��ȭ ����</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// ���ο� �������� �κ��丮�� �߰��ϴ� �Լ�
    /// �÷��̾ �������� �Ⱦ��� �� �ڵ����� ȣ���
    /// </summary>
    public void AddItem()
    {
        // CharacterManager���� �÷��̾ �Ⱦ��� ������ ������ ��������
        ItemData data = CharacterManager.Instance.Player.itemData;

        // ������ ������ ��ȿ�� �˻�
        if (data == null)
        {
            Debug.LogWarning("[UIInventory] ������ �����Ͱ� null�Դϴ�!");
            return;
        }

        Debug.Log($"[UIInventory] {data.displayName} �߰� �õ�");

        // ���� ������ �������� ��� ���� ���ÿ� �߰� �õ�
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;                                    // ���� ���� ���� ����
                UpdateUI();                                         // UI ������Ʈ
                CharacterManager.Instance.Player.itemData = null;  // �Ⱦ��� ������ ������ �ʱ�ȭ
                return;
            }
        }

        // �� ���� ã��
        ItemSlot emptySlot = GetEmptySlot();

        // �� ������ �ִٸ� ���ο� ������ �߰�
        if (emptySlot != null)
        {
            emptySlot.item = data;                              // ������ ������ ����
            emptySlot.quantity = 1;                             // ���� 1�� ����
            UpdateUI();                                         // UI ������Ʈ
            CharacterManager.Instance.Player.itemData = null;  // �Ⱦ��� ������ ������ �ʱ�ȭ
            return;
        }

        // �� ������ ���� �� �������� ���� ���
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    /// <summary>
    /// ��� ������ UI�� �ֽ� ���·� ������Ʈ�ϴ� �Լ�
    /// ������ �߰�, ����, ��� �� ȣ��Ǿ� ȭ���� ������
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� ������ ������ �ִٸ� UI ǥ��, ���ٸ� �ʱ�ȭ
            if (slots[i].item != null)
            {
                slots[i].Set();         // ������ ������ UI ����
            }
            else
            {
                slots[i].Clear();       // UI �ʱ�ȭ
            }
        }
    }

    /// <summary>
    /// ���� ������ �������� ���� ������ ã�� �Լ�
    /// ���� �������̸鼭 �ִ� ���� ������ �������� ���� ������ ��ȯ
    /// </summary>
    /// <param name="data">ã�� ������ ������</param>
    /// <returns>���� ������ ���� �Ǵ� null</returns>
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
    /// ����ִ� ������ ã�� �Լ�
    /// </summary>
    /// <returns>�� ���� �Ǵ� null</returns>
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
    /// �������� ���忡 ����ϴ� �Լ�
    /// �κ��丮�� ���� �� ���� �÷��̾ �ǵ������� ����� �� ���
    /// </summary>
    /// <param name="data">����� ������ ������</param>
    public void ThrowItem(ItemData data)
    {
        // �������� ��� �������� �÷��̾� ��� ��ġ�� ����
        // ������ ȸ������ �����Ͽ� �ڿ������� ����߸�
        Instantiate(data.dropprefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// Ư�� ������ �������� �����ϰ� ����â�� ǥ���ϴ� �Լ�
    /// ItemSlot���� ȣ��Ǿ� ���õ� �������� �� ������ ǥ����
    /// </summary>
    /// <param name="index">������ ������ �ε���</param>
    public void SelectItem(int index)
    {
        // ������ ���Կ� �������� ������ �Լ� ����
        if (slots[index].item == null) return;

        // ���� ���õ� ������ �ƿ����� ��Ȱ��ȭ
        if (selectedItem != null)
        {
            selectedItem.equipped = false;
            selectedItem.GetComponent<Outline>().enabled = false;
        }

        // ���ο� ���� ���� �� �ε��� ����
        selectedItem = slots[index];
        selectedItemIndex = index;

        // ���� ���õ� ������ �ƿ����� Ȱ��ȭ
        selectedItem.equipped = true;
        selectedItem.GetComponent<Outline>().enabled = true;

        // ���õ� �������� �⺻ ���� ǥ��
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        // ���� ���� �ʱ�ȭ
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // �Һ� ������ �������� ���� ���� ǥ��
        for (int i = 0; i < selectedItem.item.consumable.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[i].value.ToString() + "\n";
        }

        // ������ Ÿ�Կ� ���� ��ư Ȱ��ȭ/��Ȱ��ȭ
        useButton.SetActive(selectedItem.item.itemType == ItemType.Consumable);                                  // �Һ� �����۸� ��� ��ư ǥ��
        equipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && !selectedItem.equipped);      // ���� �����ϰ� ������ ������ �� ���� ��ư ǥ��
        unEquipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && selectedItem.equipped);     // ���� �����ϰ� ���� ������ �� ���� ��ư ǥ��
        dropButton.SetActive(true);                                                                             // ��� ��ư�� �׻� ǥ��
    }

    /// <summary>
    /// ���õ� �������� ����ϴ� �Լ� (�Һ� ������ ����)
    /// ��� ��ư Ŭ�� �� ȣ���
    /// </summary>
    public void OnUseButton()
    {
        // �Һ� ������ ���������� Ȯ��
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            // �������� ��� �Һ� ȿ�� ����
            for (int i = 0; i < selectedItem.item.consumable.Length; i++)
            {
                switch (selectedItem.item.consumable[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumable[i].value);     // ü�� ȸ��
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumable[i].value);      // ����� �ؼ�
                        break;
                }
            }
            RemoveSelctedItem();    // ��� �� ������ ����
        }
    }

    /// <summary>
    /// ���õ� �������� ����ϴ� �Լ�
    /// ��� ��ư Ŭ�� �� ȣ���
    /// </summary>
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);   // �������� ���忡 ���
        RemoveSelctedItem();            // �κ��丮���� ������ ����
    }

    /// <summary>
    /// ���� ���õ� �������� �κ��丮���� �����ϴ� �Լ�
    /// ������ ����̳� ��� �� ȣ��Ǿ� ������ ���ҽ�Ű�ų� ������ ������
    /// </summary>
    void RemoveSelctedItem()
    {
        slots[selectedItemIndex].quantity--;    // ���� 1 ����

        // ������ 0 ���ϰ� �Ǹ� ������ ������ ���� ���� ����
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();          // ���õ� ������ ����â �ʱ�ȭ
        }

        UpdateUI();                             // UI ������Ʈ
    }

    /// <summary>
    /// ���� ���� �Լ� (���� �̱���)
    /// ������ �������� ������ �� ����� ����
    /// </summary>
    /// <param name="index">������ �������� ���� �ε���</param>
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        // �߰����� ���� ���� ������ ���⿡ ���� ����
    }

    /// <summary>
    /// Ư�� �������� ���� ���ο� ������ Ȯ���ϴ� �Լ� (���� �̱���)
    /// �ٸ� �ý��ۿ��� �÷��̾ Ư�� �������� ������ �ִ��� Ȯ���� �� ����� ����
    /// </summary>
    /// <param name="item">Ȯ���� ������</param>
    /// <param name="quantity">�ʿ��� ����</param>
    /// <returns>���� ����</returns>
    public bool HasItem(ItemData item, int quantity)
    {
        return false;   // ����� �׻� false ��ȯ (�̱���)
    }
}
