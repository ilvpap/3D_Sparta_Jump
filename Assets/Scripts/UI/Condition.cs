using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 개별 상태(체력, 배고픔, 스태미나 등)를 관리하는 기본 클래스
/// 각 상태는 현재값, 최대값, 시작값, 수동 변화량을 가지며 UI 바로 표시됨
/// 코드 재사용성을 위해 개별 스크립트로 작성
/// </summary>
public class Condition : MonoBehaviour
{
    [Header("상태 값 설정")]
    public float curValue;      // 현재 상태 값
    public float maxValue;      // 최대 상태 값
    public float startValue;    // 게임 시작 시 초기 상태 값
    public float passiveValue;  // 시간당 자동으로 변화하는 값 (양수: 증가, 음수: 감소)

    [Header("UI 연결")]
    public Image uiBar;         // 상태를 표시할 UI 바 (Fill Amount로 퍼센테이지 표시)

    /// <summary>
    /// 초기화 - 현재 값을 시작 값으로 설정
    /// </summary>
    private void Start()
    {
        curValue = startValue;
    }

    /// <summary>
    /// 매 프레임마다 UI 바의 Fill Amount를 현재 상태 퍼센테이지로 업데이트
    /// </summary>
    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    /// <summary>
    /// 상태 값을 증가시키는 함수 (회복, 충전 등에 사용)
    /// </summary>
    /// <param name="amount">증가시킬 양</param>
    public void Add(float amount)
    {
        // 현재값 + 증가량과 최대값 중 더 작은 값을 선택 (최대값 초과 방지)
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    /// <summary>
    /// 상태 값을 감소시키는 함수 (데미지, 소모 등에 사용)
    /// </summary>
    /// <param name="amount">감소시킬 양</param>
    public void Subtract(float amount)
    {
        // 현재값 - 감소량과 0 중 더 큰 값을 선택 (0 미만으로 떨어지는 것 방지)
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    /// <summary>
    /// 현재 상태의 퍼센테이지를 계산하여 반환
    /// UI 바의 fillAmount나 다른 시스템에서 사용
    /// </summary>
    /// <returns>0.0 ~ 1.0 사이의 퍼센테이지 값</returns>
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
