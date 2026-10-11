using UnityEngine;
using TMPro;

public class SWhackMonster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float score;
    Rigidbody2D rb;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Whack();
        }
    }

    private void Whack()
    {
        rb.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            score += 100;
            scoreText.text = "Score: " + score;
        }
    }
}
