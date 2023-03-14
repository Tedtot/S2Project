using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //objects
    private GameObject player;
    private GameObject camera;

    //movement variables
    private float playerSpeed;
    private Rigidbody2D rb;
    private Vector2 movementDirection;

    private Vector3 targetPos;
    private Vector3 velocity;


    void Start() {
        player = GameObject.Find("Player");
        camera = GameObject.Find("Main Camera");

        playerSpeed = 2.5f;
        rb = player.GetComponent<Rigidbody2D>();

        velocity = Vector3.zero;
    }

    void Update() { 
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        targetPos = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
    }

    void FixedUpdate() {
        rb.velocity = movementDirection * playerSpeed;
    }
    private void LateUpdate() {
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPos, ref velocity, 0.2f);
    }

}
    