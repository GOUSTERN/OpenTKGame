using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
    internal class Transform
    {
        private Vector3 _posititon;
        public Vector3 Posititon
        {
            get { return _posititon; }
            set
            {
                _posititon = value;
                _translationMatrix = Matrix4.CreateTranslation(_posititon);
                _isDirty = true;
            }
        }

        private Vector3 _scale;
        public Vector3 Scale
        {
            get { return Scale; }
            set
            {
                _scale = value;
                _scaleMatrix = Matrix4.CreateScale(_scale);
                _isDirty = true;
            }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get { return Rotation; }
            set
            {
                _rotation = value;
                _rotationMatrix = Matrix4.CreateFromQuaternion(_rotation);
                _isDirty = true;
            }
        }

        private Matrix4 _translationMatrix;
        private Matrix4 _scaleMatrix;
        private Matrix4 _rotationMatrix;

        private Matrix4 _transformMatrix;

        private bool _isDirty;
        
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

        public void ForceUpdate()
        {
            _transformMatrix = _rotationMatrix * _translationMatrix * _scaleMatrix;
        }

        public void Move(Vector3 move)
        {
            Posititon = _posititon + move;
        }

        public void Rotate(Vector3 euler)
        {
            Console.WriteLine(euler);
            Console.WriteLine(euler * MathHelper.Pi / 180.0f);
            Rotation = _rotation * Quaternion.FromEulerAngles(euler * MathHelper.Pi / 180.0f);
        }

        public void Rotate(Vector3 axis, float angle)
        {
            Rotation = _rotation * Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angle));
        }

        public ref Matrix4 GetTransformMatrix()
        {
            return ref _transformMatrix;
        }
    }
}