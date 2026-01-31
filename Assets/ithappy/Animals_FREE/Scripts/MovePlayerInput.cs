using UnityEngine;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(CreatureMover))]
    public class MovePlayerInput : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private string m_HorizontalAxis = "Horizontal";
        [SerializeField] private string m_VerticalAxis = "Vertical";
        [SerializeField] private string m_JumpButton = "Jump";
        [SerializeField] private KeyCode m_RunKey = KeyCode.LeftShift;

        [Header("Camera")]
        [SerializeField] private PlayerCamera m_Camera;
        [SerializeField] private string m_MouseX = "Mouse X";
        [SerializeField] private string m_MouseY = "Mouse Y";
        [SerializeField] private string m_MouseScroll = "Mouse ScrollWheel";

        [Header("Move Axis Lock (World)")]
        [Tooltip("ONにすると移動方向をワールド軸に固定します（W=+X など）")]
        [SerializeField] private bool lockMoveToWorldAxis = true;

        [Tooltip("Wで進む方向（ワールド方向） 例：(1,0,0) で +X")]
        [SerializeField] private Vector3 worldForward = Vector3.right; // ★W=+Xにしたいなら right

        private CreatureMover m_Mover;

        private Vector2 m_Axis;
        private bool m_IsRun;
        private bool m_IsJump;

        private Vector3 m_Target;
        private Vector2 m_MouseDelta;
        private float m_Scroll;

        private void Awake()
        {
            m_Mover = GetComponent<CreatureMover>();
        }

        private void Update()
        {
            GatherInput();
            SetInput();
        }

        public void GatherInput()
        {
            m_Axis = new Vector2(Input.GetAxis(m_HorizontalAxis), Input.GetAxis(m_VerticalAxis));
            m_IsRun = Input.GetKey(m_RunKey);
            m_IsJump = Input.GetButton(m_JumpButton);

            // ★ここが重要：移動基準（Target）をどうするか
            if (lockMoveToWorldAxis)
            {
                // 「自分の位置 + ワールド前方向」をターゲットにする
                // これで W/S は worldForward に対して前後、A/D はその直交方向に
                Vector3 f = worldForward;
                f.y = 0f;
                if (f.sqrMagnitude < 0.0001f) f = Vector3.right;
                f.Normalize();

                m_Target = transform.position + f; // ★常にワールド基準
            }
            else
            {
                // 従来：カメラ基準（カメラが斜めだとWが斜めになる）
                m_Target = (m_Camera == null) ? Vector3.zero : m_Camera.Target;
            }

            m_MouseDelta = new Vector2(Input.GetAxis(m_MouseX), Input.GetAxis(m_MouseY));
            m_Scroll = Input.GetAxis(m_MouseScroll);
        }

        public void BindMover(CreatureMover mover)
        {
            m_Mover = mover;
        }

        public void SetInput()
        {
            if (m_Mover != null)
            {
                m_Mover.SetInput(in m_Axis, in m_Target, in m_IsRun, m_IsJump);
            }

            if (m_Camera != null)
            {
                m_Camera.SetInput(in m_MouseDelta, m_Scroll);
            }
        }
    }
}
