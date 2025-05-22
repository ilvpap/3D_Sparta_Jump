using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ķ�����̾� ������Ʈ - ���� �� ������ ���� ������ ��ü�鿡�� ���������� �������� ���ϴ� Ŭ����
/// </summary>
public class CampFire : MonoBehaviour
{
    [Header("������ ����")]
    public int Damage;          // �� ���� ���� ������ ��
    public float DamageRate;    // �������� �ִ� ���� (�� ����)

    // ���� ķ�����̾� ���� ���� �ִ� ���� ������ ��ü���� �����ϴ� ����Ʈ
    List<IDamagable> things = new List<IDamagable>();


    /// <summary>
    /// ���� ���� �� ȣ�� - ������ �ֱ� ����
    /// </summary>
    void Start()
    {
        // DamageRate �������� DealDamage �Լ��� �ݺ� ȣ�� (��� ����, ���� �ֱ��� ����)
        InvokeRepeating("DealDamage", 0, DamageRate);
    }

    /// <summary>
    /// ���� �� ��� ���� ������ ��ü�鿡�� �������� �ִ� �Լ�
    /// InvokeRepeating�� ���� �ֱ������� ȣ���
    /// </summary>
    void DealDamage()
    {
        // ����Ʈ�� ����� ��� ���� ������ ��ü���� ��ȸ
        for (int i = 0; i < things.Count; i++)
        {
            // �� ��ü���� ���� ������ ����
            things[i].TakePhysicalDamage(Damage);
        }
    }

    /// <summary>
    /// Ʈ���� ������ ��ü�� ������ �� ȣ��
    /// IDamagable �������̽��� ������ ��ü��� ������ ��� ����Ʈ�� �߰�
    /// </summary>
    /// <param name="other">Ʈ���ſ� ���� �ݶ��̴�</param>
    private void OnTriggerEnter(Collider other)
    {
        // ���� ��ü�� IDamagable ������Ʈ�� ������ �ִ��� Ȯ��
        if (other.TryGetComponent(out IDamagable damagable))
        {
            // ������ ��� ����Ʈ�� �߰�
            things.Add(damagable);
        }
    }

    /// <summary>
    /// Ʈ���� �������� ��ü�� ������ �� ȣ��
    /// �ش� ��ü�� ������ ��� ����Ʈ���� ����
    /// </summary>
    /// <param name="other">Ʈ���ſ��� ���� �ݶ��̴�</param>
    private void OnTriggerExit(Collider other)
    {
        // ���� ��ü�� IDamagable ������Ʈ�� ������ �ִ��� Ȯ��
        if (other.TryGetComponent(out IDamagable damagable))
        {
            // ������ ��� ����Ʈ���� ����
            things.Remove(damagable);
        }
    }
}
