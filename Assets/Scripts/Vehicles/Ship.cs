using System;
using System.Collections;
using UnityEngine;

namespace Vehicles
{
    public abstract class Ship : BaseVehicle
    {
        public override bool isCollideHeight(float height)
        {
            return collisionHeight < height;
        }

        public override void OnBridgeCrossing()
        {
            // Do nothing
        }
        
        public virtual IEnumerator FlyAwayRoutine(float height = 12f, float time = 1.0f, Action callback = null)
        {
            float passedTime = 0f;
            gfx.GetComponent<SpriteRenderer>().sortingLayerName = "Effect";
            while (passedTime < time)
            {
                float dy = Mathf.Lerp(0, height, passedTime/time);
                gfx.transform.localPosition = new Vector3(0, dy,0);
                passedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            OnDeath();
            callback?.Invoke();
        }

        public override void OnWait()
        {
            state = VehicleState.MoveAfter;
            currentTime = 0f;
        }

        public override void OnCollisionFront()
        {
            Debug.Log("Front Collided");
        }

        public override void OnCollisionUp()
        {
            Debug.Log("Middle Collided");
        }

        public override void OnArrival()
        {
            Destroy(gameObject);
        }
    }
}

