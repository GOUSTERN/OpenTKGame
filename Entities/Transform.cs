using OpenTK.Mathematics;

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
                _translationMatrix = Matrix4.CreateTranslation(_posititon);
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
                _scaleMatrix = Matrix4.CreateScale(_scale);
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
                _rotationMatrix = Matrix4.CreateFromQuaternion(_rotation);
                _isDirty = true;
            }
        }

        protected Matrix4 _translationMatrix;
        public ref Matrix4 GetTransformMatrix()
        {
            return ref _transformMatrix;
        }

        protected Matrix4 _scaleMatrix;
        protected Matrix4 _rotationMatrix;

        protected Matrix4 _transformMatrix;

        protected bool _isDirty;
        
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

        public virtual void Rotate(Vector3 euler)
        {
            Rotation = _rotation * Quaternion.FromEulerAngles(euler * MathHelper.Pi / 180.0f);
        }

        public virtual void Rotate(Vector3 axis, float angle)
        {
            Rotation = _rotation * Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angle));
        }

        public virtual Vector3 Up()
        {
            return (_rotationMatrix * Vector4.UnitY).Xyz;
        }

        public virtual Vector3 Right()
        {
            return (_rotationMatrix * Vector4.UnitX).Xyz;
        }

        public virtual Vector3 Forward()
        {
            return (_rotationMatrix * Vector4.UnitZ * -1.0f).Xyz;
        }
    }
}