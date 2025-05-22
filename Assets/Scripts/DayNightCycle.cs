using UnityEngine;

/// <summary>
/// ���� ���� ����Ŭ�� �����ϰ� �¾�� ���� ������ �����ϴ� Ŭ����
/// �ð��� ���� ���� ����, ����, ������ �ڵ����� ����
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Header("�ð� ����")]
    // ���� �ð��� 0 ~ 1�� ǥ�� (0: ����, 0.5: ����, 1: �ٽ� ����)
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;     // �Ϸ� ��ü ���� (�� ����)
    public float startTime = 0.4f;  // ���� ���� �� �ʱ� �ð�
    private float timeRate;         // �ð� ���� ���� (�ʴ� time ������)
    public Vector3 noon;            // ���� �� �¾��� ���� ������

    [Header("�¾� ���� ����")]
    public Light sun;                           // �¾� Light ������Ʈ
    public Gradient sunColor;                   // �ð��� ���� �¾� ���� �׶���Ʈ
    public AnimationCurve sunIntensity;         // �ð��� ���� �¾� ���� ���� Ŀ��

    [Header("�� ���� ����")]
    public Light moon;                          // �� Light ������Ʈ
    public Gradient moonColor;                  // �ð��� ���� �� ���� �׶���Ʈ
    public AnimationCurve moonIntensity;        // �ð��� ���� �� ���� ���� Ŀ��

    [Header("��ü ���� ����")]
    public AnimationCurve lightingIntensityMultiplier;      // ��ü ȯ�汤 ���� ���� Ŀ��
    public AnimationCurve reflectionIntensityMultiplier;    // �ݻ籤 ���� ���� Ŀ��

    /// <summary>
    /// �ʱ�ȭ - �ð� ���� ��� �� ���� �ð� ����
    /// </summary>
    private void Start()
    {
        // �Ϸ� ��ü ���̸� ������� �ð� ���� ���� ���
        timeRate = 1.0f / fullDayLength;
        // ���� ���� �ð� ����
        time = startTime;
    }

    /// <summary>
    /// �� �����Ӹ��� �ð� ������Ʈ �� ���� ���� ����
    /// </summary>
    private void Update()
    {
        // �ð��� ���������� ������Ű�� 1.0�� ������ 0���� ��ȯ
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // �¾�� ���� ���� ���� ������Ʈ
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // ��ü ȯ�� ���� ����
        // Evaluate�� Ŀ�꿡�� ���� time�� �ش��ϴ� ���� ��ȯ
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);      // ȯ�汤 ����
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // �ݻ籤 ����
    }

    /// <summary>
    /// ���� ����(�¾� �Ǵ� ��)�� ���¸� ������Ʈ�ϴ� �Լ�
    /// </summary>
    /// <param name="lightSource">������Ʈ�� Light ������Ʈ</param>
    /// <param name="colorGradiant">�ð��� ���� ���� �׶���Ʈ</param>
    /// <param name="intensityCurve">�ð��� ���� ���� Ŀ��</param>
    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        // ���� �ð��� ���� ���� ���� ���
        float intensity = intensityCurve.Evaluate(time);

        // ���� ���� ���
        // �Ϸ� �ð�(0 ~ 1)�� ��/���� �����ֱ�(0 ~ 360)�� ���� ����ȭ
        // �ؿ� ���� 180�� ���̰� ���Ƿ� �¾��� 0.25f(90��), ���� 0.75f(270��) ������ ����
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;

        // �ð��� ���� ���� ���� ����
        lightSource.color = colorGradiant.Evaluate(time);

        // ���� ���� ����
        lightSource.intensity = intensity;

        // ���� ������ 0�̸� ��Ȱ��ȭ, 0���� ũ�� Ȱ��ȭ (���� ����ȭ)
        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);        // ������ 0�̰� ���� Ȱ��ȭ�Ǿ� ������ ��Ȱ��ȭ
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);         // ������ 0���� ũ�� ���� ��Ȱ��ȭ�Ǿ� ������ Ȱ��ȭ
    }
}