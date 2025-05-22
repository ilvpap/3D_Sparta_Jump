using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾��� ���� ����(ü��, �����, ���¹̳� ��)�� �����ϴ� �⺻ Ŭ����
/// �� ���´� ���簪, �ִ밪, ���۰�, ���� ��ȭ���� ������ UI �ٷ� ǥ�õ�
/// �ڵ� ���뼺�� ���� ���� ��ũ��Ʈ�� �ۼ�
/// </summary>
public class Condition : MonoBehaviour
{
    [Header("���� �� ����")]
    public float curValue;      // ���� ���� ��
    public float maxValue;      // �ִ� ���� ��
    public float startValue;    // ���� ���� �� �ʱ� ���� ��
    public float passiveValue;  // �ð��� �ڵ����� ��ȭ�ϴ� �� (���: ����, ����: ����)

    [Header("UI ����")]
    public Image uiBar;         // ���¸� ǥ���� UI �� (Fill Amount�� �ۼ������� ǥ��)

    /// <summary>
    /// �ʱ�ȭ - ���� ���� ���� ������ ����
    /// </summary>
    private void Start()
    {
        curValue = startValue;
    }

    /// <summary>
    /// �� �����Ӹ��� UI ���� Fill Amount�� ���� ���� �ۼ��������� ������Ʈ
    /// </summary>
    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    /// <summary>
    /// ���� ���� ������Ű�� �Լ� (ȸ��, ���� � ���)
    /// </summary>
    /// <param name="amount">������ų ��</param>
    public void Add(float amount)
    {
        // ���簪 + �������� �ִ밪 �� �� ���� ���� ���� (�ִ밪 �ʰ� ����)
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    /// <summary>
    /// ���� ���� ���ҽ�Ű�� �Լ� (������, �Ҹ� � ���)
    /// </summary>
    /// <param name="amount">���ҽ�ų ��</param>
    public void Subtract(float amount)
    {
        // ���簪 - ���ҷ��� 0 �� �� ū ���� ���� (0 �̸����� �������� �� ����)
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    /// <summary>
    /// ���� ������ �ۼ��������� ����Ͽ� ��ȯ
    /// UI ���� fillAmount�� �ٸ� �ý��ۿ��� ���
    /// </summary>
    /// <returns>0.0 ~ 1.0 ������ �ۼ������� ��</returns>
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
