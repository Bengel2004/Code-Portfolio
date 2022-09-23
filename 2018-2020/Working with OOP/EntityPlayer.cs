using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : Entity
{
    protected override void OnDeath()
    {
        base.OnDeath();
        GameManager.Instance.OnDeath();
    }

    // Resets game level on death
    public override void DamageEntity(float damagePoints)
    {
        base.DamageEntity(damagePoints);
        if(health <= 0)
        {
            OnDeath();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.transform.gameObject)
        {
            CameraShake.OnShake?.Invoke();
        }
    }
}
