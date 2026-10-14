using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<ItemType> OnItemCaught;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHealthChanged;
    public static event Action OnGameOver;

    public static void ItemCaught(ItemType type) => OnItemCaught?.Invoke(type);
    public static void ScoreChanged(int score) => OnScoreChanged?.Invoke(score);
    public static void HealthChanged(int hp) => OnHealthChanged?.Invoke(hp);
    public static void GameOver() => OnGameOver?.Invoke();
}

public enum ItemType { Good, Bomb }