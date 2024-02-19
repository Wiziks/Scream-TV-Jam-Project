using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class RestoringSprintLine : MonoBehaviour {
    private UnityEngine.UI.Image _image;

    public void OnRestoring(Component component, object data) {
        if (data is not float progress) return;

        progress = Mathf.Clamp01(progress);

        transform.localScale = new Vector3(progress, 1f, 1f);

        if (_image == null) {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        _image.enabled = progress != 1f;
    }
}
