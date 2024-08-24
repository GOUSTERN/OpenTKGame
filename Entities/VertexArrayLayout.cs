﻿using OpenTK.Graphics.OpenGL4;
using System.Collections;
using System.Drawing;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
     internal class VertexArrayLayout : IEnumerable<VertexArrayAttribute>
    {
        public int Stride { get; private set; }

        private List<VertexArrayAttribute> _attributes;

        public VertexArrayLayout()
        {
            Stride = 0;
            _attributes = new List<VertexArrayAttribute>();
        }

        public void AddAttribute(VertexArrayAttribute attribute)
        {
            _attributes.Add(attribute);
            Stride += attribute.Size * attribute.SizeofType;
        }

        public IEnumerator<VertexArrayAttribute> GetEnumerator() => _attributes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal struct VertexArrayAttribute
    {
        public VertexAttribPointerType Type;
        public int Size;
        public bool normalized;
        public int SizeofType;
    }
}