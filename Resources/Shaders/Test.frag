#version 330 core

out vec4 FragColor;

//in vec2 textureCoord;
in float zCord;

//uniform sampler2D texture0;

void main()
{
    FragColor = vec4(zCord, zCord, zCord, 1.0f); //texture(texture0, textureCoord);
}