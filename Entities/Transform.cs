using System.Numerics;

namespace OpenTKGame.Core
{
    public class Transform
    {
        protected Vector3 _posititon;
        public virtual Vector3 Posititon
        {
            get { return _posititon; }
            set
            {
                _posititon = value;
                _translationMatrix = Matrix4x4.CreateTranslation(_posititon);
                _isDirty = true;
            }
        }

        protected Vector3 _scale;
        public virtual Vector3 Scale
        {
            get { return Scale; }
            set
            {
                _scale = value;
                _scaleMatrix = Matrix4x4.CreateScale(_scale);
                _isDirty = true;
            }
        }

        protected Quaternion _rotation;
        public virtual Quaternion Rotation
        {
            get { return Rotation; }
            set
            {
                _rotation = value;
                _rotationMatrix = Matrix4x4.CreateFromQuaternion(_rotation);
                _isDirty = true;
            }
        }

        protected Matrix4x4 _translationMatrix;
        public ref Matrix4x4 GetTransformMatrix()
        {
            return ref _transformMatrix;
        }

        protected Matrix4x4 _scaleMatrix;
        protected Matrix4x4 _rotationMatrix;

        protected Matrix4x4 _transformMatrix;

        protected bool _isDirty;
        public virtual bool IsDirty
        {
            get { return _isDirty; }
        }

        public Transform()
        {
            Posititon = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;

            _isDirty = true;
        }

        public Transform(Vector3 posititon, Vector3 scale, Quaternion rotation)
        {
            Posititon = posititon;
            Scale = scale;
            Rotation = rotation;

            _isDirty = true;
        }

        public void Update()
        {
            if (_isDirty)
                ForceUpdate();
        }

        public virtual void ForceUpdate()
        {
            _transformMatrix = _scaleMatrix * _rotationMatrix * _translationMatrix;
            _isDirty = false;
        }

        public virtual void Move(Vector3 move)
        {
            Posititon = _posititon + move;
        }

        public virtual void Rotate(Vector3 axis, float angle)
        {
            Rotation = _rotation * Quaternion.CreateFromAxisAngle(axis, angle * MathF.PI / 180.0f);
        }

        public Vector3 Right()
        {
            return new Vector3(_rotationMatrix.M11, _rotationMatrix.M21, _rotationMatrix.M31);
        }
        
        public Vector3 Up()
        {
            return new Vector3(_rotationMatrix.M12, _rotationMatrix.M22, _rotationMatrix.M33);
        }

        public Vector3 Forward()
        {
            return new Vector3(-_rotationMatrix.M13, -_rotationMatrix.M23, -_rotationMatrix.M33);
        }
    }
}