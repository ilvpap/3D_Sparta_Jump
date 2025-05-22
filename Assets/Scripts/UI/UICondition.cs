using UnityEngine;

// �÷��̾��� ��� ���� ���ǵ��� �ϳ��� ��� �����ϴ� UI ��Ʈ�ѷ� Ŭ����
// ü��, �����, ���¹̳� �� ���� Condition���� �׷�ȭ�Ͽ� ����
// PlayerCondition���� �� Ŭ������ �����Ͽ� �� ���¿� ������
public class UICondition : MonoBehaviour
{
    [Header("�÷��̾� ���� UI")]
    public Condition health;    // ü�� ���� UI �� ���� ����
    public Condition hunger;    // ����� ���� UI �� ���� ����  
    public Condition stamina;   // ���¹̳� ���� UI �� ���� ����

    // �ʱ�ȭ - PlayerCondition�� �� UICondition �ν��Ͻ��� ���
    // PlayerCondition�� �� ���¿� ������ �� �ֵ��� ������ ����
    private void Start()
    {
        // CharacterManager�� ���� �÷��̾��� condition ������Ʈ�� �� UI�� ����
        // �̷��� �ϸ� PlayerCondition���� health, hunger, stamina�� ���� ���� ����
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}