using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Effect : MonoBehaviour
    {
        public static Effect Instance { get; private set; }

        [Header("Set in Inspector")]
        [SerializeField] private GameObject heartParticle;
        [SerializeField] private float _lifeTime;

        void Start()
        {
            Instance = this;
        }

        public void HeartDestroy(Vector2 position)
        {
            GameObject effect = Instantiate(heartParticle, position, Quaternion.identity);
            Destroy(effect, _lifeTime);
        }
    }
}