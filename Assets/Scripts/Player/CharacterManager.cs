using UnityEngine;

/// <summary>
/// �÷��̾� ĳ���͸� ���������� �����ϴ� �̱��� �Ŵ��� Ŭ����
/// ������ ��� �������� �÷��̾� ������ ������ �� �ֵ��� �߾� ���߽� ������ ����
/// �� ��ȯ �ÿ��� �����Ǵ� DontDestroyOnLoad ������Ʈ
/// </summary>
public class CharacterManager : MonoBehaviour
{
    // �̱��� ������ ���� ���� �ν��Ͻ� ����
    private static CharacterManager _instance;

    /// <summary>
    /// �̱��� �ν��Ͻ��� �����ϱ� ���� ���� ������Ƽ
    /// �ν��Ͻ��� ������ �ڵ����� �����Ͽ� ��ȯ (Lazy Initialization)
    /// </summary>
    public static CharacterManager Instance
    {
        get
        {
            // �ν��Ͻ��� �������� ������ ���� ����
            if (_instance == null)
            {
                // "CharacterManager"��� �̸��� GameObject ���� �� �� ������Ʈ �߰�
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // �÷��̾� �ν��Ͻ��� �����ϴ� private �ʵ�
    public Player _player;

    /// <summary>
    /// �÷��̾� �ν��Ͻ��� �����ϱ� ���� ���� ������Ƽ
    /// �ٸ� ��ũ��Ʈ���� �÷��̾� ������ ���� �������� ������ �� �ֵ��� ��
    /// </summary>
    public Player Player
    {
        get { return _player; }         // �÷��̾� �ν��Ͻ� ��ȯ
        set { _player = value; }        // �÷��̾� �ν��Ͻ� ����
    }

    /// <summary>
    /// ������Ʈ ���� �� ȣ��Ǵ� �ʱ�ȭ �Լ�
    /// �̱��� ���� ���� �� �� ��ȯ �� �ı� ���� ����
    /// </summary>
    private void Awake()
    {
        // �̹� �ν��Ͻ��� �������� �ʴ� ��쿡�� �� ������Ʈ�� �ν��Ͻ��� ����
        if (_instance == null)
        {
            _instance = this;                           // ���� ������Ʈ�� �̱��� �ν��Ͻ��� ����
            DontDestroyOnLoad(gameObject);              // �� ��ȯ �ÿ��� �� ������Ʈ�� �ı����� �ʵ��� ����
        }
        else
        {
            // �̹� �ν��Ͻ��� �����ϸ� �ߺ� ������ ������Ʈ�� �ı�
            // �̱��� ������ �����ϱ� ���� �ϳ��� �ν��Ͻ��� �����ϵ��� ����
            Destroy(gameObject);
        }
    }
}
