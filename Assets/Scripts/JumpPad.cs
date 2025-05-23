using UnityEngine;

/// <summary>
/// 점프 구조물 (점프대) 스크립트
/// 플레이어가 점프대에 닿았을 때 위 방향으로 순간적인 힘을 가함
/// ForceMode.Impulse를 사용하여 즉각적인 점프 효과를 줌
/// </summary>

[RequireComponent(typeof(Collider))]
public class JumpPad : MonoBehaviour
{
    [Header("점프 파워 설정")]
    [Tooltip("캐릭터에게 가해질 점프 힘")]
    public float jumpForce = 150f;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트에 Rigidbody가 붙어 있는지 확인
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Y 방향 속도를 초기화해서 점프 힘이 더 잘 전달되도록 설정
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;

            // 위쪽으로 순간적인 힘을 가함 (Impulse)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Debug.Log($"[JumpPad] {collision.collider.name} → 점프대 작동! 힘: {jumpForce}");
        }
    }
}