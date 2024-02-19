using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class FlashlightStatus : MonoBehaviour {
    private UnityEngine.UI.Image _image;

    public void FlashlightTimer(Component component, object data) {
        if (data is not float progress) return;

        if (_image == null) {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        _image.fillAmount = progress;

        _image.enabled = progress != 1f;
    }
}
