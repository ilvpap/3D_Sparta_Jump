using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾ �������� �޾��� �� ȭ�鿡 ���� ���� ȿ���� ǥ���ϴ� Ŭ����
/// �������� ������ ������ �������̰� ��Ÿ���ٰ� ������ ������� �ð��� �ǵ�� ����
/// </summary>
public class DamageIndicator : MonoBehaviour
{
    [Header("UI ����")]
    public Image imagee;            // ������ ǥ�ÿ� UI �̹��� (ȭ�� ��ü ��������)
    public float flashSpeed;        // ������ ������� �ӵ� (�������� ������ �����)

    private Coroutine coroutine;    // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ ����

    /// <summary>
    /// �ʱ�ȭ - �÷��̾��� ������ �̺�Ʈ�� Flash �Լ��� ����
    /// </summary>
    void Start()
    {
        // �÷��̾ �������� ���� ������ Flash �Լ��� �ڵ����� ȣ��ǵ��� �̺�Ʈ ����
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    /// <summary>
    /// ������ ���� ȿ���� �����ϴ� �Լ�
    /// �÷��̾ �������� ���� �� �ڵ����� ȣ���
    /// </summary>
    public void Flash()
    {
        // �̹� ���� ���� ���̵� �ƿ� �ڷ�ƾ�� �ִٸ� �ߴ�
        // (���ο� �������� ���� �� ���� ȿ���� �����ϱ� ����)
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // ������ ǥ�� �̹��� Ȱ��ȭ
        imagee.enabled = true;

        // �ʱ� ���� ���� (������ �迭, RGB: 255, 100, 100)
        imagee.color = new Color(1f, 100f / 255f, 100f / 255f);

        // ���̵� �ƿ� �ڷ�ƾ ����
        coroutine = StartCoroutine(FadeAway());
    }

    /// <summary>
    /// ������ ���� ȿ���� ������ ������� �ϴ� �ڷ�ƾ
    /// ���� ���� ���������� ���ҽ��� ���̵� �ƿ� ȿ�� ����
    /// </summary>
    /// <returns>�ڷ�ƾ IEnumerator</returns>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;    // ���� ���� �� (����)
        float a = startAlpha;       // ���� ���� ��

        // ���� ���� 0���� Ŭ ������ �ݺ�
        while (a > 0)
        {
            // �ð��� ���� ���� ���� ���������� ����
            // flashSpeed�� Ŭ���� ������ �����
            a -= (startAlpha / flashSpeed) * Time.deltaTime;

            // �̹��� ���� ������Ʈ (RGB�� �����ϰ� ���ĸ� ����)
            imagee.color = new Color(1f, 100f / 255f, 100f / 255f, a);

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���̵� �ƿ� �Ϸ� �� �̹��� ��Ȱ��ȭ (���� ����ȭ)
        imagee.enabled = false;
    }
}
