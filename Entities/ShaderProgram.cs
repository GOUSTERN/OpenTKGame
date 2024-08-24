using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
    internal class ShaderProgram : IDisposable
    {
        private int _program;
        
        public ShaderProgram()
        {
            _program = GL.CreateProgram();
            GL.LinkProgram(_program);
        }

        public ShaderProgram(FragmentShader fragmentShader, VertexShader vertexShader)
        {
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
    }
}