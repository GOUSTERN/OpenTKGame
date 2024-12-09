using System.Numerics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using OpenTKGame.Graphics;

namespace OpenTKGame.Core
{
    public class Game : GameWindow
    {
        private int windowWidth;
        private int windowHeight;

        private Formatters.ModelData model;

        private VertexArray vertexArray;
        private ShaderProgram shaderProgram;
        //private Texture texture;

        private Transform transform;
        private Camera camera;

        public Game(int width, int height, string title)
                 : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            (windowWidth, windowHeight) = (width, height);
            CenterWindow(new OpenTK.Mathematics.Vector2i(width, height));
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            model = Formatters.ObjectFileReader.ReadFile("monkey.obj", 1)[0];

            VertexBuffer vertexBuffer = new VertexBuffer();
            vertexBuffer.Use();
            vertexBuffer.BufferData(model.Vertices.ToArray(), sizeof(float));

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
            vertexArray.SpecifyAttributeLayout(layout);
            
            ElementBuffer elementBuffer = new ElementBuffer();
            elementBuffer.Use();
            elementBuffer.BufferData(model.Indices.ToArray(), sizeof(uint));

            VertexShader vertexShader = new VertexShader();
            vertexShader.Compile(LoadShaderSource("Test.vert"));

            FragmentShader fragmentShader = new FragmentShader();
            fragmentShader.Compile(LoadShaderSource("Test.frag"));

            shaderProgram = new ShaderProgram(fragmentShader, vertexShader);
            shaderProgram.LinkProgram();
            shaderProgram.Use();

            /*StbImage.stbi_set_flip_vertically_on_load(1);

            texture = new Texture(TextureTarget.Texture2D);

            texture.Use();
            texture.SetWrapping(TextureWrapMode.Repeat);
            texture.SetFiltering(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            texture.LoadTexture(new Texture.LoadTextureArgs("bricksx64.png"));
            shaderProgram.SetInt("texture0", 0);*/

            vertexShader.Dispose();
            fragmentShader.Dispose();

            transform = new Transform();
            transform.ForceUpdate();

            CursorState = CursorState.Grabbed;
            camera = new Camera(74.0f, (float)windowWidth / windowHeight);
            camera.transform.Move(Vector3.UnitZ * -3);
            camera.transform.ForceUpdate();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            vertexArray.Dispose();
            shaderProgram.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(System.Drawing.Color.DarkKhaki);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            vertexArray.Use();
            //texture.Use(TextureUnit.Texture0);
            shaderProgram.Use();
            Matrix4x4 mvp = transform.GetTransformMatrix() * camera.GetViewProjectionMatrix();
            shaderProgram.SetMatrix4("mvp", ref mvp);
            
            GL.DrawElements(PrimitiveType.Triangles, model.Indices.Count, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //transform.Rotate(Vector3.UnitX + Vector3.UnitZ + Vector3.UnitY, (float)e.Time * 50f);

            transform.Update();
            camera.Update();

            if (!IsFocused)
                return;

            KeyboardState keyboard = KeyboardState;
            MouseState mouse = MouseState;

            Vector3 dir = new Vector3();
            float speed = 3f;

            if (keyboard.IsKeyDown(Keys.W))
                dir -= camera.transform.Forward();

            if (keyboard.IsKeyDown(Keys.S))
                dir += camera.transform.Forward();

            if (keyboard.IsKeyDown(Keys.A))
                dir += camera.transform.Right();

            if (keyboard.IsKeyDown(Keys.D))
                dir -= camera.transform.Right();

            if (keyboard.IsKeyDown(Keys.Space))
                dir -= Vector3.UnitY;

            if (keyboard.IsKeyDown(Keys.LeftControl))
                dir += Vector3.UnitY;

            if (dir.LengthSquared() >= 0.1f)
                camera.transform.Move(dir / dir.Length() * speed * (float)e.Time);

            if (mouse.Delta.LengthSquared >= 0.001f)
            {
                camera.transform.Rotate(Vector3.UnitY, mouse.Delta.X * 0.05f);
                camera.transform.Rotate(camera.transform.Right(), mouse.Delta.Y * 0.04f);
            }

            //Console.WriteLine(1 / e.Time);
            //Console.WriteLine($"forward: { camera.transform.Forward() }");

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                if (CursorState == CursorState.Normal)
                    Close();
                else
                    CursorState = CursorState.Normal;
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            (windowWidth, windowHeight) = (e.Width, e.Height);
            camera.ResetProjection(74.0f, (float)windowWidth / windowHeight);
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