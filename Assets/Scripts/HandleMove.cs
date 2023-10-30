using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleMove : MonoBehaviour
{
    [SerializeField] private Transform handle;
    [SerializeField] private Transform origin;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float circleRadius;
    private Vector3 orbitPosition;
    private float angle;
    private int direction;
    private int coinsCollected;

    private void Awake() => direction = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            direction = -direction;
        orbitPosition = AngleToPos() * circleRadius;
        handle.position = origin.position + orbitPosition;
        angle += Time.deltaTime * rotationSpeed * direction;
    }

    private Vector3 AngleToPos() =>
        new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);


    private void CollectCoin(GameObject coin)
    {
        coin.SetActive(false);
        coinsCollected++;
        Debug.Log("COINS " + coinsCollected);
    }

    private void FinishGame()
    {
        Debug.Log("GG");
        SceneManager.LoadSceneAsync(0);
        Emitter.IsGameFinished = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pick"))
            CollectCoin(other.gameObject);
        if (other.gameObject.CompareTag("Obstacle"))
            FinishGame();
    }
}
