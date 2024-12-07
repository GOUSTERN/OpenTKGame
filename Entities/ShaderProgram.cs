using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OpenTKGame.Graphics
{
    internal class ShaderProgram : IDisposable
    {
        private Dictionary<string, int> _uniforms;
        private int _program;

        public ShaderProgram()
        {
            _program = GL.CreateProgram();
            GL.LinkProgram(_program);
        }

        public ShaderProgram(FragmentShader fragmentShader, VertexShader vertexShader)
        {
            _uniforms = new Dictionary<string, int>();
            _program = GL.CreateProgram();

            AttachShaders(fragmentShader, vertexShader);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_program);
        }

        public void AttachShaders(FragmentShader fragmentShader, VertexShader vertexShader)
        {
            GL.AttachShader(_program, fragmentShader.GlShader);
            GL.AttachShader(_program, vertexShader.GlShader);
        }

        public void LinkProgram()
        {
            GL.LinkProgram(_program);
        }

        public void Use()
        {
            GL.UseProgram(_program);
        }

        public void SetInt(string uniformName, int value)
        {
            GL.Uniform1(TryGetUniform(uniformName), value);
        }

        public void SetVector4(string uniformName, Vector4 vector)
        {
            GL.Uniform4(TryGetUniform(uniformName), vector);
        }

        public void SetMatrix4(string uniformName, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(TryGetUniform(uniformName), true, ref matrix);
        }

        private int TryGetUniform(string name)
        {
            if (!_uniforms.ContainsKey(name))
                _uniforms[name] = GL.GetUniformLocation(_program, name);

            if (_uniforms[name] < 0)
                throw new Exception($"Uniform with name {name} cannot be found");

            return _uniforms[name];
        }
    }
}