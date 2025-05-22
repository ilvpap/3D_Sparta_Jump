using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캠프파이어 오브젝트 - 범위 내 접촉한 피해 가능한 객체들에게 지속적으로 데미지를 가하는 클래스
/// </summary>
public class CampFire : MonoBehaviour
{
    [Header("데미지 설정")]
    public int Damage;          // 한 번에 가할 데미지 량
    public float DamageRate;    // 데미지를 주는 간격 (초 단위)

    // 현재 캠프파이어 범위 내에 있는 피해 가능한 객체들을 저장하는 리스트
    List<IDamagable> things = new List<IDamagable>();


    /// <summary>
    /// 게임 시작 시 호출 - 데미지 주기 시작
    /// </summary>
    void Start()
    {
        // DamageRate 간격으로 DealDamage 함수를 반복 호출 (즉시 시작, 이후 주기적 실행)
        InvokeRepeating("DealDamage", 0, DamageRate);
    }

    /// <summary>
    /// 범위 내 모든 피해 가능한 객체들에게 데미지를 주는 함수
    /// InvokeRepeating에 의해 주기적으로 호출됨
    /// </summary>
    void DealDamage()
    {
        // 리스트에 저장된 모든 피해 가능한 객체들을 순회
        for (int i = 0; i < things.Count; i++)
        {
            // 각 객체에게 물리 데미지 적용
            things[i].TakePhysicalDamage(Damage);
        }
    }

    /// <summary>
    /// 트리거 영역에 객체가 들어왔을 때 호출
    /// IDamagable 인터페이스를 구현한 객체라면 데미지 대상 리스트에 추가
    /// </summary>
    /// <param name="other">트리거에 들어온 콜라이더</param>
    private void OnTriggerEnter(Collider other)
    {
        // 들어온 객체가 IDamagable 컴포넌트를 가지고 있는지 확인
        if (other.TryGetComponent(out IDamagable damagable))
        {
            // 데미지 대상 리스트에 추가
            things.Add(damagable);
        }
    }

    /// <summary>
    /// 트리거 영역에서 객체가 나갔을 때 호출
    /// 해당 객체를 데미지 대상 리스트에서 제거
    /// </summary>
    /// <param name="other">트리거에서 나간 콜라이더</param>
    private void OnTriggerExit(Collider other)
    {
        // 나간 객체가 IDamagable 컴포넌트를 가지고 있는지 확인
        if (other.TryGetComponent(out IDamagable damagable))
        {
            // 데미지 대상 리스트에서 제거
            things.Remove(damagable);
        }
    }
}
