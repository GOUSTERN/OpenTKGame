#version 330 core

out vec4 FragColor;
in vec3 oColors;

void main()
{
    FragColor = vec4(oColors, 1.0f);
}