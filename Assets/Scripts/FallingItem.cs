using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [SerializeField] ItemType type;
    [SerializeField] float fallSpeed = 4f;
    [SerializeField] float killY = -6f;

    [Tooltip("Optional: one of these is picked at random when the item spawns (used for the kitties).")]
    [SerializeField] Sprite[] possibleSprites;

    void Start()
    {
        if (possibleSprites != null && possibleSprites.Length > 0)
        {
            GetComponent<SpriteRenderer>().sprite =
                possibleSprites[Random.Range(0, possibleSprites.Length)];
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        if (transform.position.y < killY) Destroy(gameObject); // missed it
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        GameEvents.ItemCaught(type);
        Destroy(gameObject);
    }
}