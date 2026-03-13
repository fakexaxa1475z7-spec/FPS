using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BiomeAnimalSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BiomeAnimals
    {
        public BiomeType biome;
        public GameObject[] animals;
    }

    public BiomeAnimals[] biomeAnimals;

    public float spawnRadius = 80f;
    public float spawnDelay = 5f;
    public int maxAnimals = 20;

    int currentAnimals = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnAnimal), 2f, spawnDelay);
    }

    void SpawnAnimal()
    {
        if (currentAnimals >= maxAnimals) return;

        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, spawnRadius, NavMesh.AllAreas))
        {
            BiomeArea biome = GetBiome(hit.position);

            if (biome == null) return;

            GameObject prefab = GetAnimalForBiome(biome.biome);

            if (prefab == null) return;

            Instantiate(prefab, hit.position, Quaternion.identity);

            currentAnimals++;
        }
    }

    BiomeArea GetBiome(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, 2f);

        foreach (Collider c in hits)
        {
            BiomeArea biome = c.GetComponent<BiomeArea>();

            if (biome != null)
                return biome;
        }

        return null;
    }

    GameObject GetAnimalForBiome(BiomeType biome)
    {
        foreach (var b in biomeAnimals)
        {
            if (b.biome == biome)
            {
                if (b.animals.Length == 0) return null;

                return b.animals[Random.Range(0, b.animals.Length)];
            }
        }

        return null;
    }
}