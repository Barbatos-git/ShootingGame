using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public BackgroundScrollManager backgroundManager;

    public static GameManager Instance;
    public Collider2D mapCollider { get; private set; }
    public Bounds MapBounds { get; private set; }

    public UnityEvent OnDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGame();
    }

    void Awake()
    {
        Instance = this;

        // マップのCollider 和 Boundsを取得する
        GameObject map = GameObject.Find("Map");
        if (map != null)
        {
            mapCollider = map.GetComponent<Collider2D>();
            if (mapCollider != null)
            {
                MapBounds = mapCollider.bounds;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        backgroundManager.SetScrolling(true);
    }

    public void PauseGame()
    {
        backgroundManager.SetScrolling(false);
    }

    public void GameOverIsBoss()
    {
        StartCoroutine(DelayedGameOver(1.5f, true));
    }

    public void GameOverIsPlayer()
    {
        StartCoroutine(DelayedGameOver(1f, false));
    }

    IEnumerator DelayedGameOver(float delay, bool isBoss)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<GameEndController>()?.EndGame(isBoss);
    }
}
