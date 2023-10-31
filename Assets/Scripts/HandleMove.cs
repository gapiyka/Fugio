using UnityEngine;
using TMPro;
using Fugio.Audio;

namespace Fugio
{
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
        [Header("Canvas")]
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private SoundFeedback sound;

        private Vector3 orbitPosition;
        private float angle;
        private int direction;
        private int collectedCoins;

        private void Awake() => direction = 1;

        private void Update()
        {
            if (SceneManager.IsGameFinished) return;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                direction = -direction;
                sound.PlaySound(SoundType.Swing);
            }
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
            SceneManager.IsGameFinished = true;
            sceneManager.SwitchToResultScreen(collectedCoins);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pick"))
                CollectCoin(other.gameObject);
            if (other.gameObject.CompareTag("Obstacle"))
                FinishGame();
        }
    }
}