using UnityEngine;

public class PowerScanBullet : MonoBehaviour
{
    public float moveSpeed;
    public float maxLifetime;
    
    float lifetime = 0;
    
    public void GoTowards(Vector2 dest)
    {
        var dir = Vector3.Normalize(new Vector3(dest.x, dest.y, 0f) - transform.position);
        
        var angle = Vector2.Angle(transform.right, new Vector2(dir.x, dir.y));
        if (dir.y < 0) angle = -angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        GetComponent<Rigidbody2D>().velocity = dir * moveSpeed;
    }
    
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime) Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}