using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenTKGame.Core
{
    public class Texture : IDisposable
    {
        public ImageResult ImageResult;

        private int _textureId;
        private TextureTarget _textureTarget;

        public Texture(TextureTarget textureTarget = TextureTarget.Texture2D)
        {
            _textureTarget = textureTarget;
            _textureId = GL.GenTexture();
        }

        public void SetWrapping(TextureWrapMode textureWrap)
        {
            GL.TexParameter(_textureTarget, TextureParameterName.TextureWrapS, (int)textureWrap);
            GL.TexParameter(_textureTarget, TextureParameterName.TextureWrapT, (int)textureWrap);
        }

        public void SetFiltering(TextureMinFilter textureMinFilter, TextureMagFilter textureMagFilter)
        {
            GL.TexParameter(_textureTarget, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(_textureTarget, TextureParameterName.TextureMagFilter, (int)textureMagFilter);
        }

        public void GenerateMipmaps(GenerateMipmapTarget generateMipmapTarget, TextureMinFilter mipmapFilter)
        {
            GL.TexParameter(_textureTarget, TextureParameterName.TextureMinFilter, (int)mipmapFilter);
            GL.TexParameter(_textureTarget, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(generateMipmapTarget);
        }

        public void LoadTexture(LoadTextureArgs loadTextureArgs)
        {
            ImageResult = ImageResult.FromStream(File.OpenRead(Directory.GetCurrentDirectory() + "\\Resources\\Textures\\" + loadTextureArgs.FilePath), loadTextureArgs.ColorComponents);
            GL.TexImage2D (
                _textureTarget, loadTextureArgs.Level,
                PixelInternalFormat.Rgba,
                ImageResult.Width, ImageResult.Height,
                0,
                loadTextureArgs.PixelFormat, PixelType.UnsignedByte,
                ImageResult.Data
            );
        }

        public void Dispose()
        {
            GL.DeleteTexture(_textureId);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(_textureTarget, _textureId);
        }

        public struct LoadTextureArgs
        {
            public string FilePath;
            public int Level = 0;
            public ColorComponents ColorComponents = ColorComponents.RedGreenBlueAlpha;
            public PixelFormat PixelFormat = PixelFormat.Rgba;

            public LoadTextureArgs(string filePath)
            {
                FilePath = filePath;
            }
        }
    }
}