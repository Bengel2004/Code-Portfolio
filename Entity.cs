using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float GetHealth
    {
        get { return health; }
    }
    public float damage;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected EntityType typeOfEntity;
    public EntityType lastCollidedType;

    protected ObjectPooler smokePool;

    // checks if entity is damaged
    public virtual void DamageEntity(float damagePoints)
    {
        health -= damagePoints;
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void Awake()
    {
        smokePool = GameObject.Find("SmokePool").GetComponent<ObjectPooler>();
    }

    protected virtual void OnDeath()
    {
        smokePool.GetNext(0, transform.position, transform.rotation);
    }

    // Detects colission with objects
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject)
        { 
            Entity _tempEntity = collision.transform.gameObject.GetComponent<Entity>();
            //      _tempEntity?.health = health;
            if (_tempEntity != null)
            {
                lastCollidedType = _tempEntity.typeOfEntity;
                DamageEntity(_tempEntity.damage);
            }
        }
    }

    // Spawns particles on destruction and adds score if object is asteroid
    private void OnDisable()
    {
        if (health <= 0)
        {
            OnDeath();
        }
    }
}

public enum EntityType
{
    Asteroid,
    LargeAsteroid,
    Projectile,
    Player
}
