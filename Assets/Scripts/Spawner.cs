using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject goodPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float interval = 1f;
    [SerializeField] float xRange = 8f;
    [Range(0f, 1f)] [SerializeField] float bombChance = 0.3f;

    float timer;
    bool active = true;

    void OnEnable()  => GameEvents.OnGameOver += Stop;
    void OnDisable() => GameEvents.OnGameOver -= Stop;
    void Stop() => active = false;

    void Update()
    {
        if (!active) return;
        timer += Time.deltaTime;
        if (timer < interval) return;
        timer = 0f;

        var prefab = Random.value < bombChance ? bombPrefab : goodPrefab;
        var pos = new Vector3(Random.Range(-xRange, xRange), 7f, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}