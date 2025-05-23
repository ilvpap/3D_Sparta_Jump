using UnityEngine;

/// <summary>
/// ���� ������ (������) ��ũ��Ʈ
/// �÷��̾ �����뿡 ����� �� �� �������� �������� ���� ����
/// ForceMode.Impulse�� ����Ͽ� �ﰢ���� ���� ȿ���� ��
/// </summary>

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("���� �Ŀ� ����")]
    [Tooltip("ĳ���Ϳ��� ������ ���� ��")]
    public float jumpForce = 150f;

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� Rigidbody�� �پ� �ִ��� Ȯ��
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Y ���� �ӵ��� �ʱ�ȭ�ؼ� ���� ���� �� �� ���޵ǵ��� ����
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;

            // �������� �������� ���� ���� (Impulse)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Debug.Log($"[JumpPad] {collision.collider.name} �� ������ �۵�! ��: {jumpForce}");
        }
    }
}