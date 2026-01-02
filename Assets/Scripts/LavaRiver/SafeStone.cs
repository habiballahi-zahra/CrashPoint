using System.Collections;
using UnityEngine;

public class SafeStone : MonoBehaviour
{
    [Header("Idle Movement")]
    public float idleAmplitude = 0.5f;
    public float idleSpeed = 2f;

    [Header("Sink Settings")]
    public float sinkDepth = 2f;
    public float sinkSpeed = 2f;
    public float sinkDelay = 0.5f;

    private Vector3 startPos;
    private bool playerOnStone;
    private Coroutine sinkRoutine;

    private void Start()
    {
        // ذخیره موقعیت اولیه سنگ
        startPos = transform.position;
    }

    private void Update()
    {
        // اگر پلیر روی سنگ نیست → حرکت idle
        if (!playerOnStone)
        {
            float yOffset = Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;
            transform.position = new Vector3(
                startPos.x,
                startPos.y + yOffset,
                startPos.z
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // پلیر روی سنگ اومد
        if (other.CompareTag("Player"))
        {
            playerOnStone = true;

            // شروع فرآیند غرق شدن
            sinkRoutine = StartCoroutine(SinkStone());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // پلیر از روی سنگ رد شد
        if (other.CompareTag("Player"))
        {
            playerOnStone = false;

            // توقف غرق شدن
            if (sinkRoutine != null)
                StopCoroutine(sinkRoutine);

            // بازگشت سنگ به موقعیت اولیه
            StartCoroutine(ReturnToStart());
        }
    }

    IEnumerator SinkStone()
    {
        // تأخیر قبل از غرق شدن
        yield return new WaitForSeconds(sinkDelay);

        Vector3 targetPos = startPos + Vector3.down * sinkDepth;

        // حرکت به سمت پایین
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                sinkSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    IEnumerator ReturnToStart()
    {
        // بازگشت نرم به موقعیت اولیه
        while (Vector3.Distance(transform.position, startPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPos,
                sinkSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
