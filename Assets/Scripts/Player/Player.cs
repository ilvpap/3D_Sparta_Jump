using System;
using UnityEngine;

/// <summary>
/// �÷��̾��� �ٽ� ������Ʈ���� ���� �����ϴ� ���� �÷��̾� Ŭ����
/// �÷��̾� ���� ��� ������Ʈ���� �߾� ��� ������ �ϸ�
/// CharacterManager�� �ڽ��� ����Ͽ� ���� ������ �����ϵ��� ��
/// </summary>
public class Player : MonoBehaviour
{
    [Header("�÷��̾� �ٽ� ������Ʈ")]
    public PlayerController controller;     // �÷��̾� �̵�, ����, ī�޶� ���� ���� ����ϴ� ��Ʈ�ѷ�
    public PlayerCondition condition;       // �÷��̾��� ü��, �����, ���¹̳� �� ���¸� �����ϴ� ������Ʈ

    [Header("������ �ý���")]
    public ItemData itemData;               // ���� �Ⱦ��� �������� ������ (�ӽ� �����)
    public Action addItem;                  // ������ �߰� �� ȣ��Ǵ� �׼� �̺�Ʈ (�κ��丮 �ý��۰� ����)

    [Header("���� ��ȣ�ۿ�")]
    public Transform dropPosition;          // �������� ����� �� ������ ��ġ (�÷��̾� ����)

    /// <summary>
    /// ������Ʈ ���� �� �ʱ�ȭ �Լ�
    /// CharacterManager�� �� �÷��̾� �ν��Ͻ��� ����ϰ�
    /// �ʿ��� ������Ʈ���� ������ ȹ����
    /// </summary>
    private void Awake()
    {
        // CharacterManager�� �̱��� �ν��Ͻ��� �� �÷��̾ ���
        // �ٸ� ��ũ��Ʈ���� CharacterManager.Instance.Player�� ���� �����ϰ� ��
        CharacterManager.Instance.Player = this;

        // ���� ���ӿ�����Ʈ�� attached�� ������Ʈ���� ���� ȹ��
        controller = GetComponent<PlayerController>();      // �÷��̾� ��Ʈ�ѷ� ������Ʈ ����
        condition = GetComponent<PlayerCondition>();        // �÷��̾� ���� ���� ������Ʈ ����
    }
}
