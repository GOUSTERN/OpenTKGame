using OpenTK.Graphics.OpenGL4;

namespace OpenTKGame.Graphics
{
    internal class VertexArray : IDisposable
    {
        private int _vao;

        public VertexArray()
        {
            _vao = GL.GenVertexArray();
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
        }

        public void Use()
        {
            GL.BindVertexArray(_vao);
        }

        public void SpecifyAttributeLayout(VertexArrayLayout vertexArrayLayout)
        {
            int stride = vertexArrayLayout.Stride;
            int offset = 0;
            int attributeNumber = 0;
            
            foreach (VertexArrayAttribute attribute in vertexArrayLayout)
            {
                EnableAttribute(attributeNumber);
                GL.VertexAttribPointer(attributeNumber, attribute.Size, attribute.Type, attribute.normalized, stride, offset);

                offset += attribute.Size * attribute.SizeofType;
                attributeNumber++;
            }
        }

        public void EnableAttribute(int attribute)
        {
            GL.EnableVertexArrayAttrib(_vao, attribute);
        }
    }
}