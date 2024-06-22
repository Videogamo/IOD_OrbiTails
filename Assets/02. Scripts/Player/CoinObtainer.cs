using System;
using UnityEngine;

public class CoinObtainer : MonoBehaviour {
    [SerializeField]
    [Tooltip("The audio source with the coin sound")]
    private AudioSource _coinAudio;

    // Particle systems for different materials
    [SerializeField]
    [Tooltip("Particle system prefab for wood collection")]
    private ParticleSystem _woodParticlesPrefab;

    [SerializeField]
    [Tooltip("Particle system prefab for metal collection")]
    private ParticleSystem _metalParticlesPrefab;

    [SerializeField]
    [Tooltip("Particle system prefab for minerals collection")]
    private ParticleSystem _mineralsParticlesPrefab;

    public Action<int[], CoinBehaviour.MaterialType> OnCoinObtained;
    private int[] _coins = new int[3];

    public int[] Coins => _coins;

    // Play the specified particle system at the given position
    private void PlayParticles(ParticleSystem particleSystemPrefab, Vector3 position) {
        if (particleSystemPrefab != null) {
            ParticleSystem instance = Instantiate(particleSystemPrefab, position, Quaternion.identity);
            instance.Play();
            Destroy(instance.gameObject, instance.main.duration);
        }
    }

    // When the player collides with a trigger
    private void OnTriggerEnter(Collider other) {
        // If the trigger is of type coin
        if (other.gameObject.CompareTag("Coin")) {
            // Get the CoinBehaviour component and invoke its CoinGot method
            CoinBehaviour coinBehaviour = other.GetComponent<CoinBehaviour>();
            coinBehaviour.CoinGot();

            // Increment the coin count and invoke the OnCoinObtained event
            _coins[(int)coinBehaviour.Material]++;
            OnCoinObtained?.Invoke(_coins, coinBehaviour.Material);

            // Play the generic coin sound
            if (_coinAudio != null) _coinAudio.Play();

            // Get the position of the coin for the particle effect
            Vector3 particlePosition = other.transform.position;

            // Play the corresponding particle effects based on the material type
            switch (coinBehaviour.Material) {
                case CoinBehaviour.MaterialType.MaterialA:
                    PlayParticles(_woodParticlesPrefab, particlePosition);
                    break;
                case CoinBehaviour.MaterialType.MaterialB:
                    PlayParticles(_metalParticlesPrefab, particlePosition);
                    break;
                case CoinBehaviour.MaterialType.MaterialC:
                    PlayParticles(_mineralsParticlesPrefab, particlePosition);
                    break;
            }
        }
    }
}