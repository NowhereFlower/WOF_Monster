using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class SWhackMonster : MonoBehaviour
{
    [SerializeField] private List<SMonster> monster;
    [SerializeField] private GameObject playButton;
    [SerializeField] private float startingTime = 30f;

    private float timeRemaining;
    private HashSet<SMonster> currentMonster = new HashSet<SMonster>();
    private bool playing = false;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private int score;
    Rigidbody2D rb;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }
    public void GameOver(int type)
    {
        foreach (SMonster monster in monster)
        {
            monster.StopGame();
        }
        playing = false;
        playButton.SetActive(true);
    }
    public void StartGame()
    {
        playButton.SetActive(false);

        for(int i = 0; i < monster.Count; i++)
        {
            monster[i].Hide();
            monster[i].SetIndex(i);
        }

        currentMonster.Clear();

        timeRemaining = startingTime;
        score = 0;
        playing = true;
    }
    private void Update()
    {
        if (playing)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            if(currentMonster.Count <= (score / 10))
            {
                int index = Random.Range(0, monster.Count);
                if (!currentMonster.Contains(monster[index]))
                {
                    currentMonster.Add(monster[index]);
                    monster[index].Activate(score / 10);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Whack();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit Game!");
        }
    }

    public void AddScore(int monsterIndex)
    {
        score += 1;
        timeRemaining += 1;
        currentMonster.Remove(monster[monsterIndex]);
    }
    public void Missed(int monsterIndex, bool isMonster)
    {
        if (isMonster)
        {
            timeRemaining -= 2;
        }
        currentMonster.Remove(monster[monsterIndex]);
    }

    private void Whack()
    {
        rb.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            scoreText.text = "Score: " + score;
        }
    }
}
