using OpenTK.Graphics.OpenGL4;

namespace OpenTKGame.Graphics
{
    internal abstract class OpenGlShader : IDisposable
    {
        public int GlShader { get; private set; }

        protected OpenGlShader(ShaderType shaderType)
        {
            GlShader = GL.CreateShader(shaderType);
        }

        public virtual void Dispose()
        {
            GL.DeleteShader(GlShader);
        }

        public virtual void Compile(string sourceCode)
        {
            GL.ShaderSource(GlShader, sourceCode);
            GL.CompileShader(GlShader);
        }
    }
}