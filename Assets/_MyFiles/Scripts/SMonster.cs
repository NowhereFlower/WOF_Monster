using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SMonster : MonoBehaviour
{
    [SerializeField] private Sprite Ghost;
    [SerializeField] private Sprite Dragon;
    [SerializeField] private Sprite Zombie;
    [SerializeField] private Sprite EyeMonster;
    [SerializeField] private Sprite Happy;
    [SerializeField] private Sprite Demon;
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    [SerializeField] private float show = 0.5f;
    [SerializeField] private float duration = 1f;

    private SpriteRenderer spriteRenderer;

    private bool canHit = true;


    private void Start()
    {
        StartCoroutine(IShowHide(startPosition, endPosition));
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (canHit)
        {
            StopAllCoroutines();
            StartCoroutine(IQuickHide());

            canHit = false;
        }
    }

    private void Hide()
    {
        transform.localPosition = startPosition;
    }
    private IEnumerator IShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        float elapsed = 0f;
        while (elapsed < show)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / show);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while(elapsed < show)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / show);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = start;
    }

    private IEnumerator IQuickHide()
    {
        yield return new WaitForSeconds(0.25f);

        if (!canHit)
        {
            Hide();
        }
    }



}