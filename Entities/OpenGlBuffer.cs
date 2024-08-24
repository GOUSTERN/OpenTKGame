using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
    internal abstract class OpenGlBuffer : IDisposable
    {
        private int _buffer;
        private BufferTarget _bufferType;
        

        protected OpenGlBuffer(BufferTarget bufferType)
        {
            _bufferType = bufferType;
            _buffer = GL.GenBuffer();
        }

        public virtual void Dispose()
        {
            GL.DeleteBuffer(_buffer);
        }

        public virtual void BufferData<T>(T[] data, int sizeOfDataType, BufferUsageHint usageHint = BufferUsageHint.StaticDraw) where T : unmanaged
        {
            GL.BufferData(_bufferType, data.Length * sizeOfDataType, data, usageHint);
        }

        public virtual void Use()
        {
            GL.BindBuffer(_bufferType, _buffer);
        }
    }
}