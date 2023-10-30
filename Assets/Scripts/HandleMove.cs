using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandleMove : MonoBehaviour
{
    [Header("Handle")]
    [SerializeField] private Transform handle;
    [SerializeField] private Transform origin;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float circleRadius;
    [SerializeField] private float speedApply;
    [SerializeField] private int maxSpeed;
    [Header("Counter")]
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private Animator counterAnim;
    [SerializeField] private string updateClip;
    [Header("Canvases")]
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject finalCanvas;
    [SerializeField] private GameObject newRecord;
    [SerializeField] private TextMeshProUGUI record;
    private Vector3 orbitPosition;
    private float angle;
    private int direction;
    private int collectedCoins;

    public void RestartGame()
    {
        Emitter.IsGameFinished = false;
        SceneManager.LoadSceneAsync(0);
    }

    private void Awake() => direction = 1;

    private void Update()
    {
        if (Emitter.IsGameFinished) return;
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
        collectedCoins++;
        if (rotationSpeed < maxSpeed)
            rotationSpeed += speedApply;
        counterText.text = collectedCoins.ToString();
        counterAnim.Play(updateClip);
    }

    private void FinishGame()
    {
        Debug.Log("GG");
        Emitter.IsGameFinished = true;
        record.text = collectedCoins.ToString();
        gameCanvas.SetActive(false);
        finalCanvas.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pick"))
            CollectCoin(other.gameObject);
        if (other.gameObject.CompareTag("Obstacle"))
            FinishGame();
    }
}
