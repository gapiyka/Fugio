using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coin;
    [Tooltip("Time in seconds between emitting")]
    [SerializeField] private float obstacleSpawnDelay;
    [Tooltip("How many obstacles should be spawned")]
    [SerializeField] private int spawnAmount;
    [Tooltip("Vertical (abs(y)) offset in which may spawn obstacle")]
    [SerializeField] private float heightOffset;
    [Tooltip("Units per second")]
    [SerializeField] private float speed;

    public static bool IsGameFinished;

    private const int mltpl = 30;

    private float coinSpawnDelay;
    private Vector3 coinRotation;
    private int spawnCounter;

    private void Start()
    {
        coinSpawnDelay = obstacleSpawnDelay / 2 + obstacleSpawnDelay;
        StartCoroutine(ObstacleSpawnPipeline());
        StartCoroutine(CoinSpawnPipeline());
    }

    private void FixedUpdate()
    {
        foreach (Transform obstacle in transform)
            obstacle.position +=
                Vector3.right * speed * Time.fixedDeltaTime;
        Transform coinT = coin.transform;
        coinT.position +=
            Vector3.right * speed * Time.fixedDeltaTime;
        coinRotation = new Vector3(0f, 0f,
            (coinT.position.x - transform.position.x) * mltpl);
        coinT.rotation = Quaternion.Euler(coinRotation);
    }

    private IEnumerator ObstacleSpawnPipeline()
    {
        while (spawnCounter < spawnAmount)
        {
            Instantiate(obstaclePrefab, GetSpawnPos(), Quaternion.identity, transform);
            spawnCounter++;
            yield return new WaitForSeconds(obstacleSpawnDelay);
        }
    }
    private IEnumerator CoinSpawnPipeline()
    {
        while (!IsGameFinished)
        {
            yield return new WaitForSeconds(coinSpawnDelay);
            if (!coin.activeSelf)
            {
                coin.transform.position = GetSpawnPos();
                coin.SetActive(true);
            }
        }
    }

    private Vector3 GetSpawnPos() =>
        transform.position + Vector3.up * Random.Range(-heightOffset, heightOffset);

    private void OnTriggerEnter2D(Collider2D other) =>
        other.transform.position = GetSpawnPos();
}
