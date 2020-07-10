using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAsteroid : Entity
{
    protected ObjectPooler scrapPointsPool;

    protected override void Awake()
    {
        base.Awake();
        scrapPointsPool = GameObject.Find("scrapPool").GetComponent<ObjectPooler>();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        if (lastCollidedType != EntityType.Asteroid && lastCollidedType != EntityType.LargeAsteroid)
        {
            switch (typeOfEntity)
            {
                case EntityType.Asteroid:
                    ScoreManager.Instance.addPoint(1f);
                    scrapPointsPool.GetNext(0, transform.position, transform.rotation);
                    break;
                case EntityType.LargeAsteroid:
                    ScoreManager.Instance.addPoint(2f);
                    scrapPointsPool.GetNext(1, transform.position, transform.rotation);
                    break;
                default:
                    break;
            }
        }
    }
}
