using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    int score;

    void OnEnable()  => GameEvents.OnItemCaught += HandleCaught;
    void OnDisable() => GameEvents.OnItemCaught -= HandleCaught;

    void HandleCaught(ItemType type)
    {
        if (type != ItemType.Good) return;
        score += 10;
        GameEvents.ScoreChanged(score);
    }
}
