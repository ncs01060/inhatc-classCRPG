using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // 오브젝트
    public GameObject player;
    // 컴포넌트
    Rigidbody2D rig;
    BoxCollider2D box;
    Animator anim;
    Transform tf;
    SpriteRenderer sprite;
    Collider2D col;

    [Header("Movement")]
    Vector2 direction;
    public float moveSpeed = 3f;
    public float maxSpeed = 18f;
    public float jumpPower = 5f;

    // Jump 체크 기능
    public LayerMask platform;
    Vector2 boxSize = new Vector2(0.8f, 0.1f);
    Vector2 center;
    bool isGrounded;
    
    void Start()
    {
        player = GameObject.Find("Player");

        rig = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        isGrounded = true;
    }
    void Update()
    {
        Move();
        CheckPlatform();
    }
    void FixedUpdate()
    {
        
    }

    // 기본 이동 - (좌우 움직임)
    public void OnMove(InputAction.CallbackContext context) // 좌우 감지 + 캐릭터 좌우 전환
    {
        direction = context.ReadValue<Vector2>();
        if (context.started && direction.x == 1)
        {
            // 이동 애니메이션 변경 추가
            Debug.Log("오른쪽 이동");
            sprite.flipX = false;
        }
        if (context.started && direction.x == -1)
        {
            // 이동 애니메이션 변경 추가
            Debug.Log("왼쪽 이동");
            sprite.flipX = true;
        }
        if (context.canceled)
        {
            // 정지한 애니메이션 변경 추가
            rig.linearVelocity = new Vector2(0, rig.linearVelocity.y);
        }
    }
    void Move() // 좌우 이동 + 최대 속도 제한
    {
        rig.AddForce(Vector2.right * moveSpeed * direction, ForceMode2D.Impulse);
        // 최대 속력 제한
        if (rig.linearVelocity.x > maxSpeed)
        {
            rig.linearVelocity = new Vector2(maxSpeed, rig.linearVelocity.y);
        }
        else if (rig.linearVelocity.x < (-1)*maxSpeed)
        {
            rig.linearVelocity = new Vector2(maxSpeed*(-1), rig.linearVelocity.y);
        }
        else
        {
            // 여긴 현재 이동속도가 0일때
            // 이동 애니메이션 정지
        }
    }
    // 기본 점프
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded == true)
        {
            rig.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    void CheckPlatform() // 점프 감지
    {
        Bounds b = col.bounds;
        center = new Vector2(b.center.x, b.min.y);
        isGrounded = Physics2D.OverlapBox(center, boxSize, 0f, platform) != null;
    }
    void OnDrawGizmos() // Lay 그리기 용도
    {
        if (tf == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
