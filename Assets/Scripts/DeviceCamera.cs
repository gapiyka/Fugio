using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;

namespace Fugio
{
    public class DeviceCamera : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private RawImage ppImage;
        private WebCamTexture webCamTexture;
        private const int size = 500;
        private string picPath;

        public void Awake()
        {
            picPath = Application.persistentDataPath + "/pp.png";
            if (!File.Exists(picPath))
                return;
            Texture2D tex = new Texture2D(size, size);
            tex.LoadImage(File.ReadAllBytes(picPath));
            rawImage.texture = tex;
            ppImage.texture = tex;
        }

        public void CameraPhoto() => StartCoroutine(PhotoPipeline());

        private IEnumerator PhotoPipeline()
        {
            webCamTexture = new WebCamTexture(size, size);
            rawImage.texture = webCamTexture;
            rawImage.material.mainTexture = webCamTexture;
            webCamTexture.Play();
            yield return new WaitForSeconds(1f);
            ApplyPhoto(false);
            yield return new WaitForSeconds(1f);
            webCamTexture.Stop();
        }

        public void ApplyPhoto(bool selected)
        {
            Texture2D photo;
            Color[] pixels;
            if (selected)
            {
                photo =
                    new Texture2D(rawImage.texture.width, rawImage.texture.height);
                pixels = (rawImage.texture as Texture2D).GetPixels();
            }
            else
            {
                photo = new Texture2D(webCamTexture.width, webCamTexture.height);
                pixels = webCamTexture.GetPixels();
            }
            photo.SetPixels(pixels);
            photo.Apply();
            byte[] bytes = photo.EncodeToPNG();
            File.WriteAllBytes(picPath, bytes);
        }
    }
}