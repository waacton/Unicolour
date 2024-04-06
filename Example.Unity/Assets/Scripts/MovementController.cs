using TMPro;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const float MoveSpeed = 10;
    private const float LookSpeed = 2;
    private const float RollSpeed = 50;
    
    private bool isEnabled;
    private TMP_Text movementTextComponent;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movementTextComponent = GameObject.Find("Movement Text").GetComponent<TMP_Text>();
        SetText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isEnabled = !isEnabled;
            SetText();
        }

        if (!isEnabled) return;
        
        var moveMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
    
        // forward and backward
        var forwardMove = MoveSpeed * moveMultiplier * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * forwardMove);

        // sideways
        var sidewaysMove = MoveSpeed * moveMultiplier * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * sidewaysMove);
        
        // up and down
        var upwardsMove = MoveSpeed * moveMultiplier * Time.deltaTime * Input.GetAxis("Jump");
        transform.Translate(Vector3.up * upwardsMove);

        // mouse look
        var horizontalLook = LookSpeed * Input.GetAxis("Mouse X");
        var verticalLook = LookSpeed * -Input.GetAxis("Mouse Y");
        transform.Rotate(verticalLook, horizontalLook, 0);

        // roll left
        if (Input.GetKey(KeyCode.Q))
        {
            var roll = RollSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, roll);
        }
        
        // roll right
        if (Input.GetKey(KeyCode.E))
        {
            var roll = RollSpeed * Time.deltaTime;
            transform.Rotate(Vector3.back, roll);
        }
    }

    private void SetText()
    {
        var enabledText = isEnabled ? "on" : "off";
        movementTextComponent.text = $"Movement {enabledText}";
    }
}
