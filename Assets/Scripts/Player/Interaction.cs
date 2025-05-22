using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾ ���� �� ��ȣ�ۿ� ������ ��ü��� ��ȣ�ۿ��� �� �ְ� ���ִ� Ŭ����
/// ȭ�� �߾ӿ��� ����ĳ��Ʈ�� �߻��Ͽ� ��ȣ�ۿ� ������ ��ü�� �����ϰ�
/// UI ������Ʈ ǥ�� �� ��ȣ�ۿ� �Է��� ó����
/// </summary>
public class Interaction : MonoBehaviour
{
    [Header("��ȣ�ۿ� ���� ����")]
    public float CheckRate = 0.05f;         // ��ȣ�ۿ� ��� Ȯ�� �ֱ� (�� ����, ���� ����ȭ��)
    private float lastCheckTime;            // ���������� Ȯ���� �ð� ����
    public float maxCheckDistance;          // ��ȣ�ۿ� ������ �ִ� �Ÿ�
    public LayerMask layerMask;             // ��ȣ�ۿ� ��� ���̾� ����ũ (Ư�� ���̾ ����)

    [Header("���� ��ȣ�ۿ� ���")]
    public GameObject curInteractGameObject;    // ���� ��ȣ�ۿ� ������ ���ӿ�����Ʈ
    private IInteractable curInteractable;      // ���� ��ȣ�ۿ� ������ ��ü�� �������̽�

    [Header("UI")]
    public TextMeshProUGUI promptText;      // ��ȣ�ۿ� ������Ʈ�� ǥ���� �ؽ�Ʈ UI
    private Camera camera;                  // ����ĳ��Ʈ �߻縦 ���� ���� ī�޶� ����

    /// <summary>
    /// �ʱ�ȭ - ���� ī�޶� ���� ȹ��
    /// </summary>
    void Start()
    {
        camera = Camera.main;
    }

    /// <summary>
    /// �� �����Ӹ��� ��ȣ�ۿ� ��� Ȯ��
    /// CheckRate �ֱ�θ� �����Ͽ� ���� ����ȭ
    /// </summary>
    void Update()
    {
        // CheckRate �ð��� ������ ���� ��ȣ�ۿ� ��� Ȯ�� (���� ����ȭ)
        if (Time.time - lastCheckTime > CheckRate)
        {
            lastCheckTime = Time.time;

            // ȭ�� �߾�(ũ�ν���� ��ġ)���� ���� �߻�
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // ����ĳ��Ʈ�� ��ȣ�ۿ� ������ ��ü ����
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // ���ο� ��ȣ�ۿ� ����� �߰��� ���
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;                        // �� ������� ����
                    curInteractable = hit.collider.GetComponent<IInteractable>();           // ��ȣ�ۿ� �������̽� ȹ��
                    SetPromptText();                                                        // ������Ʈ �ؽ�Ʈ ����
                }
            }
            else
            {
                // ��ȣ�ۿ� ������ ��ü�� ���� ��� �ʱ�ȭ
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);     // ������Ʈ �ؽ�Ʈ ����
            }
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� ������Ʈ �ؽ�Ʈ�� �����ϰ� ǥ���ϴ� �Լ�
    /// ���� ������ ��ȣ�ۿ� ������ ��ü�� �̸��� ǥ����
    /// </summary>
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);                          // ������Ʈ �ؽ�Ʈ Ȱ��ȭ
        promptText.text = curInteractable.GetInteractableName();        // ��ȣ�ۿ� ��ü�� �̸� ����
    }

    /// <summary>
    /// �÷��̾ ��ȣ�ۿ� Ű�� ������ �� ȣ��Ǵ� �Լ�
    /// Input System�� �ݹ����� ����Ǿ� ����
    /// </summary>
    /// <param name="context">�Է� �׼� ���ؽ�Ʈ</param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // Ű�� ���� �����̰�, ��ȣ�ۿ� ������ ��ü�� ���� ���� ����
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();                   // ��ȣ�ۿ� ����

            // ��ȣ�ۿ� �Ϸ� �� ���� �ʱ�ȭ
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);         // ������Ʈ �ؽ�Ʈ ����
        }
    }
}
