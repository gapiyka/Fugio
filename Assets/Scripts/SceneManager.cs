using TMPro;
using UnityEngine;

namespace Fugio
{
    public class SceneManager : MonoBehaviour
    {
        [Header("MainCanvas")]
        [SerializeField] private GameObject lobbyCanvas;
        [SerializeField] private GameObject pictureCanvas;
        [SerializeField] private DeviceCamera profilePic;
        [SerializeField] private TextMeshProUGUI playerRecord;
        [SerializeField] private TMP_InputField username;
        [Header("InGameCanvas")]
        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject finalCanvas;
        [SerializeField] private GameObject newRecord;
        [SerializeField] private TextMeshProUGUI record;

        public static bool IsGameFinished;

        public static void SwitchToMainScene() =>
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);

        public static void SwitchToGameScene() =>
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        public void SwitchToResultScreen(int amount)
        {
            record.text = amount.ToString();
            gameCanvas.SetActive(false);
            finalCanvas.SetActive(true);
            if (amount > SaveManager.Info.Record)
            {
                newRecord.SetActive(true);
                SaveManager.Save(amount);
            }
            else
                newRecord.SetActive(false);
        }

        public void SwitchLobbyScreen()
        {
            if (!lobbyCanvas.activeSelf) profilePic.Awake();
            lobbyCanvas.SetActive(!lobbyCanvas.activeSelf);
            pictureCanvas.SetActive(!pictureCanvas.activeSelf);
        }

        public void SaveUsername() => SaveManager.Save(username.text);

        private void Awake()
        {
            if (pictureCanvas != null)
            {
                pictureCanvas.GetComponent<DeviceCamera>().Awake();
                PlayerInfo player = SaveManager.Load();
                playerRecord.text = player.Record.ToString();
                username.text = player.Name;
            }
        }
    }
}