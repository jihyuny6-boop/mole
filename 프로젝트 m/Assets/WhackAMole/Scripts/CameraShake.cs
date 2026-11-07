using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            // 씬이 변경되어도 파괴되지 않게 하려면 아래 주석을 해제하세요.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // 시작할 때의 원래 카메라 위치 저장
        originalPosition = transform.localPosition;
    }

    /// <summary>
    /// 외부에서 카메라 진동을 호출하는 함수
    /// </summary>
    /// <param name="duration">진동 지속 시간 (초)</param>
    /// <param name="magnitude">진동 세기</param>
    public void Shake(float duration, float magnitude)
    {
        // 이미 진동 중이면 기존 코루틴을 정지하고 위치를 복구
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalPosition;
        }

        // 새로운 진동 시작
        shakeCoroutine = StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 랜덤한 구체 내부 좌표를 생성하여 세기를 곱함
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * magnitude;

            // Z축은 카메라가 앞뒤로 흔들려 잘리는 현상을 막기 위해 기존 값 유지
            randomPoint.z = originalPosition.z;

            transform.localPosition = randomPoint;

            elapsed += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 진동이 끝나면 원래 위치로 정확히 복구
        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
