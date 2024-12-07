using OpenTK.Graphics.OpenGL4;

namespace OpenTKGame.Graphics
{
    internal class VertexBuffer : OpenGlBuffer, IDisposable
    {
        public VertexBuffer() : base(BufferTarget.ArrayBuffer) { }
    }
}