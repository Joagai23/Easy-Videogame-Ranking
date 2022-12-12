using UnityEngine;
using UnityEngine.UI;

public class LoadingData : MonoBehaviour
{
    // Private class properties
    private float rate = 0.5f;
    private float progress = 0.0f;

    // UI References
    public Image loadingCircle;

    // Update Wheel UI
    private void Update()
    {
        progress += rate * Time.deltaTime;

        if(progress > 1.0f)
        {
            progress = 0.0f;
        }

        loadingCircle.fillAmount = progress;
    }
}
