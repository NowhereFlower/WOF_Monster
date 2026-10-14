using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class SMonster : MonoBehaviour
{
    [SerializeField] private Sprite Ghost;
    [SerializeField] private Sprite Dragon;
    [SerializeField] private Sprite Zombie;
    [SerializeField] private Sprite EyeMonster;
    [SerializeField] private Sprite Happy;
    [SerializeField] private Sprite Demon;
    [SerializeField] private GameObject Bomb;

    [SerializeField] private SWhackMonster WhackManager;

    private int monsterIndex = 0;
    private BoxCollider2D boxCollider2D;
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    private Vector2 boxOffSet;
    private Vector2 boxSize;
    private Vector2 boxOffSetHidden;
    private Vector2 boxSizeHidden;
    [SerializeField] private float show = 0.5f;
    [SerializeField] private float duration = 1f;

    private SpriteRenderer spriteRenderer;

    private bool canHit = true;
    public enum MonsterType { Ghost, Dragon, Zombie, EyeMonster, Demon, Happy, Bomb};
    private MonsterType monsterType;

    private int lives;
    private float bombRate = 0f;

    public void Activate(int level)
    {
        SetLevel(level);
        CreateNext();
        StartCoroutine(IShowHide(startPosition, endPosition));
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxOffSet = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffSetHidden = new Vector2(boxOffSet.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);
    }

    public void SetIndex(int index)
    {
        monsterIndex = index;
    }
    public void StopGame()
    {
        canHit = false;
        StopAllCoroutines();
    }

    private void OnMouseDown()
    {
        if (canHit)
        {
            switch (monsterType)
            {
                case MonsterType.Ghost:
                    WhackManager.AddScore(monsterIndex);
                    StopAllCoroutines();
                    StartCoroutine(IQuickHide());

                    canHit = false;
                    break;
                case MonsterType.Zombie:
                    WhackManager.AddScore(monsterIndex);

                    StopAllCoroutines();
                    StartCoroutine(IQuickHide());

                    canHit = false;
                    break;
                case MonsterType.Happy:
                    WhackManager.AddScore(monsterIndex);

                    StopAllCoroutines();
                    StartCoroutine(IQuickHide());

                    canHit = false;
                    break;
                case MonsterType.EyeMonster:
                    WhackManager.AddScore(monsterIndex);

                    StopAllCoroutines();
                    StartCoroutine(IQuickHide());

                    canHit = false;
                    break;
                case MonsterType.Dragon:
                    if(lives == 2)
                    {
                        lives--;
                    }
                    else
                    {
                        WhackManager.AddScore(monsterIndex);

                        StopAllCoroutines();
                        StartCoroutine(IQuickHide());
                        canHit = false;
                    }
                    break;
                case MonsterType.Demon:
                    if (lives == 3 || lives == 2)
                    {
                        lives--;
                    }
                    else
                    {
                        WhackManager.AddScore(monsterIndex);
                        StopAllCoroutines();
                        StartCoroutine(IQuickHide());
                        canHit = false;
                    }
                    break;
                case MonsterType.Bomb:
                    WhackManager.GameOver(1);
                    break;
                default: break;

            }
        }
    }
    private void CreateNext()
    {
        int random = Random.Range(0, 6);
        if(random < bombRate)
        {
            monsterType = MonsterType.Bomb;
            Bomb.SetActive(true);
        }
        else { Bomb.SetActive(false); }
        if(random == 0)
        {
            monsterType = MonsterType.Ghost;
            spriteRenderer.sprite = Ghost;
            lives = 1;
        }
        if (random == 1)
        {
            monsterType = MonsterType.Dragon;
            spriteRenderer.sprite = Dragon;
            show = 1f;
            duration = 2f;
            lives = 2;
        }
        if (random == 2)
        {
            monsterType = MonsterType.Zombie;
            spriteRenderer.sprite = Zombie;
            lives = 1;
        }
        if (random == 3)
        {
            monsterType = MonsterType.EyeMonster;
            spriteRenderer.sprite = EyeMonster;
            lives = 1;
        }
        if (random == 4)
        {
            monsterType = MonsterType.Happy;
            spriteRenderer.sprite = Happy;
            lives = 1;
        }
        if (random == 5)
        {
            monsterType = MonsterType.Demon;
            spriteRenderer.sprite = Demon;
            show = 1f;
            duration = 2f;
            lives = 3;
        }
        canHit = true;
    }
    public void Hide()
    {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffSetHidden;
        boxCollider2D.size = boxSizeHidden;
    }
    private void SetLevel(int level)
    {
        bombRate = Mathf.Min(level * 0.025f, 0.25f);

        float durationMin = Mathf.Min(1 - level * 0.1f, 0.01f, 1f);
        float durationMax = Mathf.Min(2 - level * 0.1f, 0.01f, 2f);
        duration = Random.Range(durationMin, durationMax);

    }
    private IEnumerator IShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        float elapsed = 0f;
        while (elapsed < show)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / show);
            boxCollider2D.offset = Vector2.Lerp(boxOffSetHidden, boxOffSet, elapsed / show);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / show);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffSet;
        boxCollider2D.size = boxSize;
        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while(elapsed < show)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / show);
            boxCollider2D.offset = Vector2.Lerp(boxOffSet, boxOffSetHidden, elapsed / show);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / show);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = start;
        boxCollider2D.offset = boxOffSetHidden;
        boxCollider2D.size = boxSizeHidden;

        if (canHit)
        {
            canHit = false;
            WhackManager.Missed(monsterIndex, monsterType != MonsterType.Bomb);
        }
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