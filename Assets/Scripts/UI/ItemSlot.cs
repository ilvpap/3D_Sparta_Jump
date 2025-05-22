using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �κ��丮 UI�� ��Ÿ���� ���� ������ ������ �����ϴ� Ŭ����
/// �� ������ ������ ����, ������, ����, ���� ���� ���� ǥ���ϰ� ������
/// </summary>
public class ItemSlot : MonoBehaviour
{
    [Header("������ ������")]
    public ItemData item;       // �� ���Կ� ����� ������ ������

    [Header("UI ����")]
    public UIInventory inventory;               // �θ� �κ��丮 UI ����
    public Button button;                       // ���� Ŭ���� ���� ��ư ������Ʈ
    public Image icon;                          // ������ �������� ǥ���� �̹���
    public TextMeshProUGUI quatityText;         // ������ ������ ǥ���� �ؽ�Ʈ
    private Outline outline;                    // ���õ� ������ ǥ���� �ƿ����� ������Ʈ

    [Header("���� ����")]
    public int index;           // �κ��丮���� �� ��° �������� ��Ÿ���� �ε���
    public bool equipped;       // ���� ���� ���� (���� ������ �������� ���)
    public int quantity;        // ������ ���� ������

    
    // ������Ʈ �ʱ�ȭ - Outline ������Ʈ ���� ȹ��
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    
    // ������ Ȱ��ȭ�� �� ȣ�� - ���� ���¿� ���� �ƿ����� ǥ��
    
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// ���� UI ������Ʈ �Լ�
    /// ������ �����Ϳ��� �ʿ��� ������ �����ͼ� �� UI ��ҿ� ǥ��
    /// </summary>
    public void Set()
    {
        // ������ ������ Ȱ��ȭ �� ��������Ʈ ����
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        // ���� �ؽ�Ʈ ���� (1������ ���� ���� ���� ǥ��, 1���� �� ���ڿ�)
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        // �ƿ����� ǥ�� ���� ���� (���� ���õ� ���������� Ȯ��)
        if (outline != null)
        {
            outline.enabled = inventory.selectedItem == this;
        }
    }

    /// <summary>
    /// ������ ���� �Լ� - �������� ���� �� UI�� �ʱ�ȭ
    /// </summary>
    public void Clear()
    {
        item = null;                                    // ������ ������ ����
        icon.gameObject.SetActive(false);               // ������ ����
        quatityText.text = string.Empty;                // ���� �ؽ�Ʈ �ʱ�ȭ
    }

    /// <summary>
    /// ���� Ŭ�� �� ȣ��Ǵ� �Լ� (Button ������Ʈ�� OnClick �̺�Ʈ�� ����)
    /// �κ��丮�� ���� ������ ���õǾ����� �˸�
    /// </summary>
    public void OnClickButton()
    {
        // �κ��丮�� SelectItem ȣ��, ���� ������ �ε����� ����
        inventory.SelectItem(index);
    }
}