using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
    internal class ElementBuffer : OpenGlBuffer, IDisposable
    {
        public ElementBuffer() : base(BufferTarget.ElementArrayBuffer) { }
    }
}