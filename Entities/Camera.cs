using System.Numerics;

namespace OpenTKGame.Core
{
    public class Camera
    {
        public Transform transform;

        private float _aspect;
        public float Aspect
        {
            get { return _aspect; }
            set
            {
                _aspect = value;
                ResetProjection(_fovY, _aspect);
            }
        }

        private float _fovY;
        public float FovY
        {
            get { return _fovY; }
            set
            {
                _fovY = value;
                ResetProjection(_fovY, _aspect);
            }
        }

        private float _depthNear;
        public float DepthNear
        {
            get { return _depthNear; }
            set
            {
                _depthNear = value;
                ResetProjection(_fovY, _aspect);
            }
        }

        private float _depthFar;
        public float DepthFar
        {
            get { return _depthFar; }
            set
            {
                _depthFar = value;
                ResetProjection(_fovY, _aspect);
            }
        }

        private Matrix4x4 _projection;
        private bool _isDirty;

        private Matrix4x4 _viewProjection;

        public Camera(float fovY, float aspect, float depthNear = 0.1f, float depthFar = 100.0f)
        {
            _fovY = fovY;
            _aspect = aspect;
            _depthNear = depthNear;
            _depthFar = depthFar;

            ResetProjection(fovY, aspect);
            transform = new CameraTransform();
        }

        public void ResetProjection(float fovY, float aspect)
        {
            _projection = Matrix4x4.CreatePerspectiveFieldOfView(fovY * MathF.PI / 180.0f, aspect, _depthNear, _depthFar);
            _isDirty = true;
        }

        public void Update()
        {
            if (transform.IsDirty || _isDirty)
            {
                transform.Update();

                _viewProjection = transform.GetTransformMatrix() * _projection;
                _isDirty = false;
            }
        }

        public ref Matrix4x4 GetViewProjectionMatrix()
        {
            return ref _viewProjection;
        }
    }
}