#version 330 core

out vec4 FragColor;

in vec2 textureCoord;
in float zCord;

uniform sampler2D texture0;

void main()
{
    FragColor = texture(texture0, textureCoord); //vec4(zCord, zCord, zCord, 1.0f);
}