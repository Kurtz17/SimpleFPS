using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");

            Target target = collision.gameObject.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(); 
            }

            Destroy(gameObject);
        }
    }
}