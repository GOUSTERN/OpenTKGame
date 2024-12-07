namespace OpenTKGame.Core
{
    public class CameraTransform : Transform
    {
        public override void ForceUpdate()
        {
            _transformMatrix = _translationMatrix * _rotationMatrix;
            _isDirty = false;
        }
    }
}