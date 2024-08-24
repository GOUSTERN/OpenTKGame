using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKGame.Core
{
    //TODO finish
    internal class Texture : IDisposable
    {
        private int _textureId;
        private TextureTarget _textureTarget;

        public Texture(TextureTarget textureTarget)
        {
            _textureTarget = textureTarget;
            _textureId = GL.GenTexture();
        }

        public void SetWrapping(TextureWrapMode textureWrap)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrap);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrap);
        }

        public void SetFiltering(TextureMagFilter textureMagFilter, TextureMagFilter textureMinFilter)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);
        }

        public void GenerateMipmaps(GenerateMipmapTarget generateMipmapTarget, TextureMinFilter mipmapFilter)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)mipmapFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(generateMipmapTarget);
        }

        public void Dispose()
        {
            GL.DeleteTexture(_textureId);
        }

        public void Use()
        {
            GL.BindTexture(_textureTarget, _textureId);
        }
    }
}