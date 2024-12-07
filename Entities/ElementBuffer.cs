using OpenTK.Graphics.OpenGL4;

namespace OpenTKGame.Graphics
{
    internal class ElementBuffer : OpenGlBuffer, IDisposable
    {
        public ElementBuffer() : base(BufferTarget.ElementArrayBuffer) { }
    }
}