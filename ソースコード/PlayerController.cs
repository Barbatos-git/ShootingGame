using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Collider2D mapCollider;

    private Vector2 moveInput;
    private Bounds mapBounds;
    private bool isAlive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mapCollider != null)
        {
            mapBounds = mapCollider.bounds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        // 入力取得(WSADまたは方向キー)
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        Vector3 newPos = transform.position + (Vector3)(moveInput.normalized * moveSpeed * Time.deltaTime);

        // 地図の境界内に制限する
        float halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2f;

        float clampedX = Mathf.Clamp(newPos.x, mapBounds.min.x + halfWidth, mapBounds.max.x - halfWidth);
        float clampedY = Mathf.Clamp(newPos.y, mapBounds.min.y + halfHeight, mapBounds.max.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void DisableMovement()
    {
        isAlive = false;
    }
}
