using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private float rotationSpeed = 10f;

    private bool isWalking;
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalised();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
