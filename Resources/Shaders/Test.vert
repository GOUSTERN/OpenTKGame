#version 330 core

layout (location = 0) in vec3 a_position;
//layout (location = 1) in vec2 aTextureCords;

//out vec2 textureCoord;
out float zCord;

uniform mat4 mvp;

void main()
{
    gl_Position = vec4(a_position, 1.0) * mvp;
    zCord = gl_Position.z / 5.0f;
    //textureCoord = aTextureCords;
}