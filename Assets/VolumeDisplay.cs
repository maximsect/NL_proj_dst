using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VolumeDisplay : MonoBehaviour
{
        public void ChangeVolume(float volume)
    {
        float soundVol = Mathf.Lerp(0, 100, 1 + volume / 80);
        GetComponent<TextMeshProUGUI>().text = ((int)soundVol).ToString();
    }
}
