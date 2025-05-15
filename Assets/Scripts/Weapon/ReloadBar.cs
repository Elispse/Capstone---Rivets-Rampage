using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ReloadBarSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform fillBarRect;
    [SerializeField] private CanvasGroup containerCanvasGrp;
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private float reloadTime = 2f;

    // Internals
    private Vector2 startPos;
    private Vector2 endPos;
    private float timer;
    private bool isReloading;

    void Awake()
    {
        // assume fillBarRect pivot (0, .5) so x=0 is “empty”, width is full range
        startPos = new Vector2(-38.5f, fillBarRect.anchoredPosition.y);
        endPos   = new Vector2(63.4f, fillBarRect.anchoredPosition.y);
        containerCanvasGrp.alpha = 0;  // start hidden
    }

    public void BeginReload(float rTime)
    {
        reloadTime = rTime;
        timer = 0f;
        isReloading = true;
        containerCanvasGrp.alpha = 1;  // show
        fillBarRect.anchoredPosition = startPos;
    }

    void Update()
    {
        if (!isReloading) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / reloadTime);

        // move fillBarRect.x from start to end
        Vector2 pos = Vector2.Lerp(startPos, endPos, t);
        fillBarRect.anchoredPosition = pos;

        if (t >= 1f)
        {
            isReloading = false;
            // hide after a tiny delay or immediately:
            containerCanvasGrp.alpha = 0;
        }
    }
}
