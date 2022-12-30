using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        public float life = 0.2f;
        public float lifeAmount;


        private void OnEnable()
        {
            life = lifeAmount;
        }
        void Update()
        {

            life -= Time.deltaTime;

            if (life <= 0)
            {
                life = lifeAmount;
                PlayerProjectilePool.Instance.OnReleaseHitEffect(this.gameObject);




            }

        }
    }
}