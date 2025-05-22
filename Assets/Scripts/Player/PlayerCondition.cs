using System;
using UnityEngine;

/// <summary>
/// 물리적 데미지를 받을 수 있는 객체들이 구현해야 하는 인터페이스
/// 캠프파이어, 적, 함정 등 다양한 데미지 소스에서 일관된 방식으로 데미지를 적용할 수 있도록 함
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// 물리적 데미지를 받는 함수
    /// </summary>
    /// <param name="damage">받을 데미지 양</param>
    void TakePhysicalDamage(float damage);
}

/// <summary>
/// 플레이어의 모든 상태 조건(체력, 배고픔, 스태미나)을 관리하는 클래스
/// UI와 연동하여 상태 변화를 시각적으로 표시하고
/// 외부에서 플레이어 상태를 변경할 수 있는 기능을 제공함
/// IDamagable 인터페이스를 구현하여 데미지를 받을 수 있음
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("UI 연결")]
    public UICondition uiCondition;     // 플레이어 상태를 표시하는 UI 컴포넌트

    // UI Condition의 개별 상태들에 쉽게 접근하기 위한 프로퍼티들
    Condition health { get { return uiCondition.health; } }     // 체력 상태 접근용 프로퍼티
    Condition hunger { get { return uiCondition.hunger; } }     // 배고픔 상태 접근용 프로퍼티
    Condition stamina { get { return uiCondition.stamina; } }   // 스태미나 상태 접근용 프로퍼티

    [Header("생존 시스템 설정")]
    public float noHungerHealthDecay;   // 배고픔이 0일 때 체력 감소 비율 (초당 감소량, 양수 값)

    // 데미지를 받을 때 발생하는 이벤트 (데미지 인디케이터 등에서 사용)
    public event Action onTakeDamage;

    /// <summary>
    /// 매 프레임마다 플레이어 상태를 자동으로 업데이트하는 함수
    /// 배고픔 감소, 스태미나 회복, 생존 시스템 처리, 사망 체크 등을 담당
    /// </summary>
    private void Update()
    {
        // 시간이 지나면서 배고픔은 자동으로 감소 (passiveValue가 음수여야 함)
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);

        // 시간이 지나면서 스태미나는 자동으로 회복 (passiveValue가 양수여야 함)
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 배고픔이 0 미만이면 굶주림으로 인한 체력 감소 적용
        if (hunger.curValue < 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        // 체력이 0 미만이면 플레이어 사망 처리
        if (health.curValue < 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 플레이어의 체력을 회복시키는 함수
    /// 포션, 음식, 회복 아이템 등에서 호출됨
    /// </summary>
    /// <param name="amount">회복할 체력량</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// 플레이어의 배고픔을 해소시키는 함수
    /// 음식 아이템 섭취 시 호출됨
    /// </summary>
    /// <param name="amount">해소할 배고픔량</param>
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    /// <summary>
    /// 플레이어 사망 처리 함수
    /// 체력이 0 이하로 떨어졌을 때 호출됨
    /// 현재는 디버그 로그만 출력하지만, 추후 게임오버 화면이나 리스폰 로직 등을 추가할 수 있음
    /// </summary>
    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
        // TODO: 게임오버 처리, 리스폰 로직, UI 표시 등을 여기에 추가
    }

    /// <summary>
    /// IDamagable 인터페이스 구현 - 물리적 데미지를 받는 함수
    /// 캠프파이어, 적의 공격, 함정 등 다양한 데미지 소스에서 호출됨
    /// </summary>
    /// <param name="damage">받을 데미지 양</param>
    public void TakePhysicalDamage(float damage)
    {
        health.Subtract(damage);        // 체력에서 데미지만큼 차감
        onTakeDamage?.Invoke();         // 데미지를 받았음을 알리는 이벤트 발생 (데미지 인디케이터 등에서 수신)
    }
}