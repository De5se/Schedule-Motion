using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    [SerializeField] private Vector2 gameTime = new(6, 00);
    public Vector2 GameTime => gameTime;

    public event Action OnTimeChanged;

    public static GameTimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer(){
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            gameTime.y++;
            if (gameTime.y >= 60)
            {
                gameTime.x++;
            }

            gameTime.x %= 24;
            gameTime.y %= 60;
            
            OnTimeChanged?.Invoke();
        }
    }

    public static bool IsTimeEqual(Vector2 time1, Vector2 time2)
    {
        return Math.Abs(VectorToIntTime(time1) - VectorToIntTime(time2)) < 1;
    }

    public static float VectorToIntTime(Vector2 time)
    {
        return time.x * 60 + time.y;
    }
}
