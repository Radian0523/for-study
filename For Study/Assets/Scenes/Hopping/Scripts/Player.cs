using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float hoppingBaseSpeed = 2f; // 通常の跳ねる速度
    [SerializeField] float hoppingDrillSpeed = 10f; // ドリル状態の速度
    [SerializeField] float hoppingHeight = 2f; // 一定の跳ねる高さ
    [SerializeField] float hoppingGroundDistance = 0.05f; // 接地判定のしきい値
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] GameObject drillVFX;
    [SerializeField] Transform drillVFXSpawnPos;
    [SerializeField] float zoomDuration = 1f;


    private float hoppingSpeed;
    private float groundY;
    private float lastGroundY; // 最後に接地した地面の高さ
    private bool isFalling = true;
    private bool isDrilling = false;
    float targetPosY;
    float timer;
    float yAngle;

    PlayerInputs _input;



    private void Start()
    {
        _input = GetComponent<PlayerInputs>();
    }

    void Update()
    {
        Move();
        Hop();
    }

    void Move()
    {
        Vector3 moveDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

    private void Hop()
    {
        hoppingSpeed = isDrilling ? hoppingDrillSpeed : hoppingBaseSpeed;

        // 地面の高さを検出
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            groundY = hit.point.y;
        }

        // スペースキーが押されたらドリルモードにする
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isDrilling)
            {
                StopAllCoroutines();

                StartCoroutine(ChangeMainCameraFOVRoutine(90));
                isDrilling = true;
                isFalling = true; // 上昇中でもすぐ下降する
            }

            // Spaceキーが押されている間は、下に行けないように制限
            if (transform.position.y < groundY)
            {
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }

            // yAngle += Time.deltaTime * rotationSpeed;
            // transform.Rotate(transform.rotation.x, yAngle, transform.rotation.z);

            // drillVFXを生成
            Instantiate(drillVFX, drillVFXSpawnPos.position, Quaternion.identity);
        }
        else
        {
            // yAngle = 0;
            isDrilling = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(ChangeMainCameraFOVRoutine(60));

        }



        // 地面に着いた時の処理
        if (Mathf.Abs(transform.position.y - groundY) < hoppingGroundDistance)
        {
            DestroyableObject destroyableObject = hit.collider?.gameObject.GetComponent<DestroyableObject>();
            if (destroyableObject)
            {
                destroyableObject.OnDestruct();
            }

            if (!isDrilling) // ドリルモードでなければ通常の跳ねる動作
            {
                isFalling = false;
                lastGroundY = groundY; // 接地時に lastGroundY を更新
            }
        }

        // 目標位置の計算（lastGroundY を基準にする）
        targetPosY = isFalling ? groundY : lastGroundY + hoppingHeight;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosY, transform.position.z), hoppingSpeed * Time.deltaTime);

        // 上昇の最高点に達した時の処理
        if (Mathf.Abs(transform.position.y - (lastGroundY + hoppingHeight)) < hoppingGroundDistance && !isFalling)
        {
            isFalling = true;
        }
    }

    IEnumerator ChangeMainCameraFOVRoutine(float changeFOVamount)
    {
        float startFOV = Camera.main.fieldOfView;
        float targetFOV = changeFOVamount;
        float elapsedTime = 0;

        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            elapsedTime += Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        Camera.main.fieldOfView = targetFOV;
    }

    /* private void Hop1()
    {
        hoppingSpeed = isDrilling ? hoppingDrillSpeed : hoppingBaseSpeed;

        // 地面の高さを検出
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            groundY = hit.point.y;
        }

        // スペースキーが押されたらドリルモードにする
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isDrilling)
            {
                isDrilling = true;
                isFalling = true; // 上昇中でもすぐ下降する
            }
        }
        else
        {
            isDrilling = false;
        }

        // 地面に着いた時の処理
        if (Mathf.Abs(transform.position.y - groundY) < hoppingGroundDistance)
        {
            DestroyableObject destroyableObject = hit.collider?.gameObject.GetComponent<DestroyableObject>();
            if (destroyableObject)
            {
                destroyableObject.OnDestruct();
                // lastGroundY = groundY; // 接地時に lastGroundY を更新
                // isFalling = true;
            }
            if (!isDrilling) // ドリルモードでなければ通常の跳ねる動作
            {
                isFalling = false;
                lastGroundY = groundY; // 接地時に lastGroundY を更新
            }
        }
        // 目標位置の計算（lastGroundY を基準にする）
        float targetPosY = isFalling ? groundY : lastGroundY + hoppingHeight;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosY, transform.position.z), hoppingSpeed * Time.deltaTime);

        // 上昇の最高点に達した時の処理
        if (Mathf.Abs(transform.position.y - (lastGroundY + hoppingHeight)) < hoppingGroundDistance && !isFalling)
        {
            isFalling = true;
        }
    } */

}
