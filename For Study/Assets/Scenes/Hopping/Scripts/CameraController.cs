using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerBody; // プレイヤーのTransform（横回転用）
    [SerializeField] private float sensitivity = 100f; // 感度

    private float verticalRotation = 0f; // 上下回転の現在の角度
    private PlayerInputs _input; // プレイヤーの入力

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputs>();

        // マウスカーソルをロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        // マウスの入力を取得
        Vector2 lookInput = _input.look;

        // 入力を感度とTime.deltaTimeで調整
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        // プレイヤーの水平方向の回転
        playerBody.Rotate(Vector3.up * mouseX);

        // カメラの垂直方向の回転（上下）
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // 上下の回転角度制限

        // カメラの角度を適用
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
