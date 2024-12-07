using OpenTK.Mathematics;

namespace OpenTKGame.Core
{
    public class Camera
    {
        public Transform Transform;
        
        private Matrix4 _projection;

        public Camera(float fovY, float aspect, float depthNear = 0.1f, float depthFar = 100.0f)
        {
            ResetProjection(fovY, aspect, depthNear, depthFar);
            Transform = new CameraTransform();
        }

        public void ResetProjection(float fovY, float aspect, float depthNear = 0.1f, float depthFar = 100.0f)
        {
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fovY), aspect, depthNear, depthFar);
        }

        public ref Matrix4 GetViewMatrix()
        {
            return ref Transform.GetTransformMatrix();
        }

        public ref Matrix4 GetProjectionMatrix()
        {
            return ref _projection;
        }
    }
}