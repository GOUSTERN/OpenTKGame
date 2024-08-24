#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColors;

out vec3 oColors;

void main()
{
    gl_Position = vec4(aPosition, 1.0f);
    oColors = aColors;
}