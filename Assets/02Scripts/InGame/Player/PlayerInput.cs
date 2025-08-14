using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region ==================== Locomotion
    // Move
    public Vector3 MoveDir;

    // Rotate
    public Vector2 MouseDir;

    #endregion ================= /Locomotion

    private void Update()
    {
        HandleMovementInput();
        HandleRotateMouseInput();
    }

    private void HandleMovementInput()
    {
        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));   // 0 ~ 1

        if (MoveDir.magnitude > 1f) MoveDir.Normalize();    // 대각선 보정, 조이스틱같은 컨트롤러 방향 힘(민감도)도 보존하기 위해(대각선 Pull은 루트2)
    }

    private void HandleRotateMouseInput()
    {
        MouseDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
