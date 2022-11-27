using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float damage = 1;
    public float knockbackForce = 20f;
    public float moveSpeed = 500f;

    public DetectionZone detectionZone;
    Rigidbody2D rb;

    DamageableCharacter damagableCharacter;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        
    }

    void FixedUpdate() {
        if(damagableCharacter.Targetable && detectionZone.detectedObjs.Count > 0) {
            // Calculate direction to target object
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;

            // Move towards detected object
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    /// Deal damage and knockback to IDamageable 
    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable != null) {
            // Offset for collision detection changes the direction where the force comes from
            Vector2 direction = (collider.transform.position - transform.position).normalized;

            // Knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
            damageable.OnHit(damage, knockback);
        }
    }
}
