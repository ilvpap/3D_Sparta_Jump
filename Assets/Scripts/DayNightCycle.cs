using UnityEngine;

/// <summary>
/// 낮과 밤의 사이클을 관리하고 태양과 달의 조명을 제어하는 클래스
/// 시간에 따라 조명 색상, 강도, 각도를 자동으로 변경
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Header("시간 설정")]
    // 현재 시간을 0 ~ 1로 표현 (0: 자정, 0.5: 정오, 1: 다시 자정)
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;     // 하루 전체 길이 (초 단위)
    public float startTime = 0.4f;  // 게임 시작 시 초기 시간
    private float timeRate;         // 시간 증가 비율 (초당 time 증가량)
    public Vector3 noon;            // 정오 때 태양의 각도 기준점

    [Header("태양 조명 설정")]
    public Light sun;                           // 태양 Light 컴포넌트
    public Gradient sunColor;                   // 시간에 따른 태양 색상 그라디언트
    public AnimationCurve sunIntensity;         // 시간에 따른 태양 조명 강도 커브

    [Header("달 조명 설정")]
    public Light moon;                          // 달 Light 컴포넌트
    public Gradient moonColor;                  // 시간에 따른 달 색상 그라디언트
    public AnimationCurve moonIntensity;        // 시간에 따른 달 조명 강도 커브

    [Header("전체 조명 설정")]
    public AnimationCurve lightingIntensityMultiplier;      // 전체 환경광 강도 조절 커브
    public AnimationCurve reflectionIntensityMultiplier;    // 반사광 강도 조절 커브

    /// <summary>
    /// 초기화 - 시간 비율 계산 및 시작 시간 설정
    /// </summary>
    private void Start()
    {
        // 하루 전체 길이를 기반으로 시간 증가 비율 계산
        timeRate = 1.0f / fullDayLength;
        // 게임 시작 시간 설정
        time = startTime;
    }

    /// <summary>
    /// 매 프레임마다 시간 업데이트 및 조명 상태 갱신
    /// </summary>
    private void Update()
    {
        // 시간을 지속적으로 증가시키며 1.0을 넘으면 0으로 순환
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // 태양과 달의 조명 상태 업데이트
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // 전체 환경 조명 설정
        // Evaluate는 커브에서 현재 time에 해당하는 값을 반환
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);      // 환경광 강도
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // 반사광 강도
    }

    /// <summary>
    /// 개별 조명(태양 또는 달)의 상태를 업데이트하는 함수
    /// </summary>
    /// <param name="lightSource">업데이트할 Light 컴포넌트</param>
    /// <param name="colorGradiant">시간에 따른 색상 그라디언트</param>
    /// <param name="intensityCurve">시간에 따른 강도 커브</param>
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        // 현재 시간에 따른 조명 강도 계산
        float intensity = intensityCurve.Evaluate(time);

        // 조명 각도 계산
        // 하루 시간(0 ~ 1)과 해/달의 자전주기(0 ~ 360)의 값을 동기화
        // 해와 달은 180도 차이가 나므로 태양은 0.25f(90도), 달은 0.75f(270도) 오프셋 적용
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;

        // 시간에 따른 조명 색상 설정
        lightSource.color = colorGradiant.Evaluate(time);

        // 조명 강도 설정
        lightSource.intensity = intensity;

        // 조명 강도가 0이면 비활성화, 0보다 크면 활성화 (성능 최적화)
        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);        // 강도가 0이고 현재 활성화되어 있으면 비활성화
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);         // 강도가 0보다 크고 현재 비활성화되어 있으면 활성화
    }
}