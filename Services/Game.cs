using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Core
{
    public class Game : GameWindow
    {
        float[] verts = {
            -0.5f,  -0.5f, 0.0f,
             0.5f,  -0.5f, 0.0f,
             0.0f,   0.5f, 0.0f
        };

        int vao;
        int shad;

        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            vao = GL.GenVertexArray();

            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            shad = GL.CreateProgram();

            int vert = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vert, LoadShaderSource("Test.vert"));
            GL.CompileShader(vert);

            int frag = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(frag, LoadShaderSource("Test.frag"));
            GL.CompileShader(frag);

            GL.AttachShader(shad, vert);
            GL.AttachShader(shad, frag);

            GL.LinkProgram(shad);

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shad);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(System.Drawing.Color.Red);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shad);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
    }
}