using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VolumeDisplay : MonoBehaviour
{
        public void ChangeVolume(float volume)
    {
        float soundVol = Mathf.Lerp(0, 100, 0.8f + volume / 50);
        GetComponent<TextMeshProUGUI>().text = ((int)soundVol).ToString();
    }
}
