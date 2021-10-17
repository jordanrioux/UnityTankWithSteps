using System.Linq;
using UnityEngine;

namespace Camera
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private float dampTime = 0.2f;
        [SerializeField] private float screenEdgeBuffer = 4f;
        [SerializeField] private float minSize = 6.5f;
        [SerializeField] private Transform[] targets;

        private UnityEngine.Camera _camera;
        private float _zoomSpeed;
        private Vector3 _moveVelocity;
        private CameraControl _cameraRig;

        public Transform[] Targets
        {
            get => targets;
            set => targets = value;
        }
        
        private void Awake()
        {
            _camera = GetComponentInChildren<UnityEngine.Camera>();
        }

        private void FixedUpdate()
        {
            var desiredPosition = CenterOnTargets();
            ZoomOnTargetsFromPosition(desiredPosition);
        }

        private Vector3 CenterOnTargets()
        {
            var desiredPosition = FindTargetsAveragePosition();
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _moveVelocity, dampTime);
            return desiredPosition;
        }

        private Vector3 FindTargetsAveragePosition()
        {
            var averagePos = targets.Aggregate(new Vector3(), (seed, target) => seed + target.position) / targets.Length;
            averagePos.y = transform.position.y;
            return averagePos;
        }

        private void ZoomOnTargetsFromPosition(Vector3 desiredPosition)
        {
            var requiredSize = FindCameraRequiredSize(desiredPosition);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, dampTime);
        }

        private float FindCameraRequiredSize(Vector3 desiredPosition)
        {
            var desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
            var size = targets.Max(target => FindTargetDistanceAsSize(target, desiredLocalPos));
            size += screenEdgeBuffer;
            size = Mathf.Max(size, minSize);
            return size;
        }

        private float FindTargetDistanceAsSize(Transform target, Vector3 desiredLocalPos)
        {
            if (!target || !target.gameObject || !target.gameObject.activeSelf)
                return 0f;

            var size = 0f;
            var targetLocalPos = transform.InverseTransformPoint(target.position);
            var desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
            return size;
        }

        public void SetStartPositionAndSize()
        {
            var desiredPosition = FindTargetsAveragePosition();
            transform.position = desiredPosition;
            _camera.orthographicSize = FindCameraRequiredSize(desiredPosition);
        }
    }
}
