﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

using OpenTKGame.Graphics;

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
        private Texture texture;

        private Transform transform;
        private Camera camera;

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

            texture = new Texture(TextureTarget.Texture2D);

            texture.Use();
            texture.SetWrapping(TextureWrapMode.Repeat);
            texture.SetFiltering(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            texture.LoadTexture(new Texture.LoadTextureArgs("bricksx64.png"));
            shaderProgram.SetInt("texture0", 0);

            vertexShader.Dispose();
            fragmentShader.Dispose();

            transform = new Transform();
            transform.ForceUpdate();

            CursorState = CursorState.Grabbed;
            camera = new Camera(74.0f, (float)WindowSize.X / WindowSize.Y);
            camera.Transform.Move(Vector3.UnitZ * -3);
            camera.Transform.ForceUpdate();
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
            texture.Use(TextureUnit.Texture0);
            shaderProgram.Use();
            shaderProgram.SetMatrix4("model", ref transform.GetTransformMatrix());
            shaderProgram.SetMatrix4("view", ref camera.GetViewMatrix());
            shaderProgram.SetMatrix4("projection", ref camera.GetProjectionMatrix());
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            transform.Update();
            camera.Transform.Update();

            if (!IsFocused)
                return;

            KeyboardState keyboard = KeyboardState;
            MouseState mouse = MouseState;

            Vector3 dir = new Vector3();
            float speed = 3f;

            if (keyboard.IsKeyDown(Keys.W))
                dir -= camera.Transform.Forward();

            if (keyboard.IsKeyDown(Keys.S))
                dir += camera.Transform.Forward();

            if (keyboard.IsKeyDown(Keys.A))
                dir += camera.Transform.Right();

            if (keyboard.IsKeyDown(Keys.D))
                dir -= camera.Transform.Right();

            if (keyboard.IsKeyDown(Keys.Space))
                dir -= Vector3.UnitY;

            if (keyboard.IsKeyDown(Keys.LeftControl))
                dir += Vector3.UnitY;

            if (dir.LengthSquared >= 0.1f)
                camera.Transform.Move(dir.Normalized() * speed * (float)e.Time);

            if (mouse.Delta.LengthSquared >= 0.001f)
            {
                camera.Transform.Rotate(Vector3.UnitY, mouse.Delta.X * 0.05f);
                camera.Transform.Rotate(camera.Transform.Right(), mouse.Delta.Y * 0.04f);
            }

            Console.WriteLine(1 / e.Time);

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
            WindowSize = (e.Width, e.Height);
            camera.ResetProjection(74.0f, (float)WindowSize.X / WindowSize.Y);
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