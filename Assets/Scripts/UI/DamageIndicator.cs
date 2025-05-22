using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어가 데미지를 받았을 때 화면에 붉은 섬광 효과를 표시하는 클래스
/// 데미지를 받으면 빨간색 오버레이가 나타났다가 서서히 사라지는 시각적 피드백 제공
/// </summary>
public class DamageIndicator : MonoBehaviour
{
    [Header("UI 설정")]
    public Image imagee;            // 데미지 표시용 UI 이미지 (화면 전체 오버레이)
    public float flashSpeed;        // 섬광이 사라지는 속도 (높을수록 빠르게 사라짐)

    private Coroutine coroutine;    // 현재 실행 중인 페이드 아웃 코루틴 참조

    /// <summary>
    /// 초기화 - 플레이어의 데미지 이벤트에 Flash 함수를 구독
    /// </summary>
    void Start()
    {
        // 플레이어가 데미지를 받을 때마다 Flash 함수가 자동으로 호출되도록 이벤트 구독
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    /// <summary>
    /// 데미지 섬광 효과를 시작하는 함수
    /// 플레이어가 데미지를 받을 때 자동으로 호출됨
    /// </summary>
    public void Flash()
    {
        // 이미 실행 중인 페이드 아웃 코루틴이 있다면 중단
        // (새로운 데미지가 들어올 때 기존 효과를 리셋하기 위함)
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // 데미지 표시 이미지 활성화
        imagee.enabled = true;

        // 초기 색상 설정 (빨간색 계열, RGB: 255, 100, 100)
        imagee.color = new Color(1f, 100f / 255f, 100f / 255f);

        // 페이드 아웃 코루틴 시작
        coroutine = StartCoroutine(FadeAway());
    }

    /// <summary>
    /// 데미지 섬광 효과를 서서히 사라지게 하는 코루틴
    /// 알파 값을 점진적으로 감소시켜 페이드 아웃 효과 구현
    /// </summary>
    /// <returns>코루틴 IEnumerator</returns>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;    // 시작 알파 값 (투명도)
        float a = startAlpha;       // 현재 알파 값

        // 알파 값이 0보다 클 때까지 반복
        while (a > 0)
        {
            // 시간에 따라 알파 값을 점진적으로 감소
            // flashSpeed가 클수록 빠르게 사라짐
            a -= (startAlpha / flashSpeed) * Time.deltaTime;

            // 이미지 색상 업데이트 (RGB는 유지하고 알파만 변경)
            imagee.color = new Color(1f, 100f / 255f, 100f / 255f, a);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 페이드 아웃 완료 후 이미지 비활성화 (성능 최적화)
        imagee.enabled = false;
    }
}
