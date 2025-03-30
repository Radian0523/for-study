using Unity.VisualScripting;
using UnityEngine;

public class PlayerTemp : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float hoppingBaseSpeed = 2f; // 通常の跳ねる速度
    [SerializeField] float hoppingDrillSpeed = 10f; // ドリル状態の速度
    [SerializeField] float hoppingHeight = 2f; // 一定の跳ねる高さ
    [SerializeField] float hoppingGroundDistance = 0.05f; // 接地判定のしきい値


    private float hoppingSpeed;
    private float groundY;
    private float lastGroundY; // 最後に接地した地面の高さ
    private bool isFalling = true;
    private bool isDrilling = false;


    Rigidbody rb;
    PlayerInputs _input;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputs>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
        rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
    }




}
