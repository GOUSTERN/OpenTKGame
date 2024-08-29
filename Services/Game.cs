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
        private Vector2i WindowSize;

        float[] verts = {
              -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
 
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
 
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
 
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
 
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
 
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        uint[] indices = {
            0, 1, 2,
        3, 4, 5,
        6, 8, 7,
        9, 11, 10,
        12, 14, 13,
        15, 17, 16,
        18, 19, 20,
        21, 22, 23,
        24, 26, 25,
        27, 29, 28,
        30, 31, 32,
        33, 34, 35,
        };

        private VertexArray vertexArray;
        private ShaderProgram shaderProgram;
        private Texture texture0;
        private Texture texture1;

        private Transform transform;
        private Matrix4 view;
        private Matrix4 projection;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            WindowSize = (width, height);
            CenterWindow(WindowSize);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

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
            shaderProgram.Use();

            StbImage.stbi_set_flip_vertically_on_load(1);

            texture0 = new Texture(TextureTarget.Texture2D);

            texture0.Use();
            texture0.SetWrapping(TextureWrapMode.Repeat);
            texture0.SetFiltering(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            texture0.LoadTexture(new Texture.LoadTextureArgs("bricksx64.png"));
            shaderProgram.SetInt("texture0", 0);

            texture1 = new Texture(TextureTarget.Texture2D);

            texture1.Use();
            texture1.SetWrapping(TextureWrapMode.Repeat);
            texture1.SetFiltering(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            texture1.LoadTexture(new Texture.LoadTextureArgs("bookshelf.png"));

            shaderProgram.SetInt("texture1", 1);

            vertexShader.Dispose();
            fragmentShader.Dispose();

            transform = new Transform();
            transform.ForceUpdate();

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)WindowSize.X / WindowSize.Y, 0.1f, 100.0f);
            view = Matrix4.CreateTranslation(new Vector3(0, 0, -3.0f));
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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            vertexArray.Use();
            texture0.Use(TextureUnit.Texture0);
            texture1.Use(TextureUnit.Texture1);
            shaderProgram.Use();
            shaderProgram.SetMatrix4("model", ref transform.GetTransformMatrix());
            shaderProgram.SetMatrix4("projection", ref projection);
            shaderProgram.SetMatrix4("view", ref view);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            transform.Rotate(Vector3.UnitY, 0.005f);
            transform.Update();

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            WindowSize = (e.Width, e.Height);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)WindowSize.X / WindowSize.Y, 0.1f, 100.0f);
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
    }
}