using System;
using UnityEngine;

/// <summary>
/// ������ �������� ���� �� �ִ� ��ü���� �����ؾ� �ϴ� �������̽�
/// ķ�����̾�, ��, ���� �� �پ��� ������ �ҽ����� �ϰ��� ������� �������� ������ �� �ֵ��� ��
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// ������ �������� �޴� �Լ�
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    void TakePhysicalDamage(float damage);
}

/// <summary>
/// �÷��̾��� ��� ���� ����(ü��, �����, ���¹̳�)�� �����ϴ� Ŭ����
/// UI�� �����Ͽ� ���� ��ȭ�� �ð������� ǥ���ϰ�
/// �ܺο��� �÷��̾� ���¸� ������ �� �ִ� ����� ������
/// IDamagable �������̽��� �����Ͽ� �������� ���� �� ����
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("UI ����")]
    public UICondition uiCondition;     // �÷��̾� ���¸� ǥ���ϴ� UI ������Ʈ

    // UI Condition�� ���� ���µ鿡 ���� �����ϱ� ���� ������Ƽ��
    Condition health { get { return uiCondition.health; } }     // ü�� ���� ���ٿ� ������Ƽ
    Condition hunger { get { return uiCondition.hunger; } }     // ����� ���� ���ٿ� ������Ƽ
    Condition stamina { get { return uiCondition.stamina; } }   // ���¹̳� ���� ���ٿ� ������Ƽ

    [Header("���� �ý��� ����")]
    public float noHungerHealthDecay;   // ������� 0�� �� ü�� ���� ���� (�ʴ� ���ҷ�, ��� ��)

    // �������� ���� �� �߻��ϴ� �̺�Ʈ (������ �ε������� ��� ���)
    public event Action onTakeDamage;

    /// <summary>
    /// �� �����Ӹ��� �÷��̾� ���¸� �ڵ����� ������Ʈ�ϴ� �Լ�
    /// ����� ����, ���¹̳� ȸ��, ���� �ý��� ó��, ��� üũ ���� ���
    /// </summary>
    private void Update()
    {
        // �ð��� �����鼭 ������� �ڵ����� ���� (passiveValue�� �������� ��)
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);

        // �ð��� �����鼭 ���¹̳��� �ڵ����� ȸ�� (passiveValue�� ������� ��)
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // ������� 0 �̸��̸� ���ָ����� ���� ü�� ���� ����
        if (hunger.curValue < 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        // ü���� 0 �̸��̸� �÷��̾� ��� ó��
        if (health.curValue < 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// �÷��̾��� ü���� ȸ����Ű�� �Լ�
    /// ����, ����, ȸ�� ������ ��� ȣ���
    /// </summary>
    /// <param name="amount">ȸ���� ü�·�</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// �÷��̾��� ������� �ؼҽ�Ű�� �Լ�
    /// ���� ������ ���� �� ȣ���
    /// </summary>
    /// <param name="amount">�ؼ��� ����ķ�</param>
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    /// <summary>
    /// �÷��̾� ��� ó�� �Լ�
    /// ü���� 0 ���Ϸ� �������� �� ȣ���
    /// ����� ����� �α׸� ���������, ���� ���ӿ��� ȭ���̳� ������ ���� ���� �߰��� �� ����
    /// </summary>
    public void Die()
    {
        Debug.Log("�÷��̾ �׾���.");
        // TODO: ���ӿ��� ó��, ������ ����, UI ǥ�� ���� ���⿡ �߰�
    }

    /// <summary>
    /// IDamagable �������̽� ���� - ������ �������� �޴� �Լ�
    /// ķ�����̾�, ���� ����, ���� �� �پ��� ������ �ҽ����� ȣ���
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public void TakePhysicalDamage(float damage)
    {
        health.Subtract(damage);        // ü�¿��� ��������ŭ ����
        onTakeDamage?.Invoke();         // �������� �޾����� �˸��� �̺�Ʈ �߻� (������ �ε������� ��� ����)
    }
}