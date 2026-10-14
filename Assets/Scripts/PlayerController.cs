using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float xLimit = 8f;

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + input * speed * Time.deltaTime, -xLimit, xLimit);
        transform.position = pos;
    }
}