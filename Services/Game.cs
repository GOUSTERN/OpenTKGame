using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace OpenTKGame.Core
{
    public class Game : GameWindow
    {
        float[] verts = {
             0.5f,  0.5f, 0.0f,     1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f,     1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f,     0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f,     0.0f, 1.0f
        };

        uint[] indices = {
            0, 1, 3,
            1, 2, 3
        };

        private VertexArray vertexArray;
        private ShaderProgram shaderProgram;
        int textureId;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            VertexBuffer vertexBuffer = new VertexBuffer();
            vertexBuffer.Use();
            vertexBuffer.BufferData(verts, sizeof(float));

            vertexArray = new VertexArray();
            vertexArray.Use();
            vertexBuffer.Use();
            
            VertexArrayLayout layout = new VertexArrayLayout();
            layout.AddAttribute(
                new VertexArrayAttribute()
                {
                    Size = 3,
                    Type = VertexAttribPointerType.Float,
                    normalized = false,
                    SizeofType = sizeof(float)
                }
            );
            layout.AddAttribute(
                new VertexArrayAttribute()
                {
                    Size = 2,
                    Type = VertexAttribPointerType.Float,
                    normalized = false,
                    SizeofType = sizeof(float)
                }
            );
            vertexArray.SpecifyAttributeLayout(layout);

            ElementBuffer elementBuffer = new ElementBuffer();
            elementBuffer.Use();
            elementBuffer.BufferData(indices, sizeof(uint));

            VertexShader vertexShader = new VertexShader();
            vertexShader.Compile(LoadShaderSource("Test.vert"));

            FragmentShader fragmentShader = new FragmentShader();
            fragmentShader.Compile(LoadShaderSource("Test.frag"));

            shaderProgram = new ShaderProgram(fragmentShader, vertexShader);
            shaderProgram.LinkProgram();

            vertexShader.Dispose();
            fragmentShader.Dispose();


            StbImage.stbi_set_flip_vertically_on_load(1);

            textureId = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            ImageResult image = ImageResult.FromStream(LoadImageStream("bricksx64.png"), ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            vertexArray.Dispose();
            shaderProgram.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(System.Drawing.Color.DarkKhaki);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shaderProgram.Use();
            vertexArray.Use();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\Resources\\Shaders\\" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
                throw new Exception("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }

        private FileStream LoadImageStream(string filePath)
        {
            return File.OpenRead(Directory.GetCurrentDirectory() + "\\Resources\\Textures\\" + filePath);
        }
    }
}