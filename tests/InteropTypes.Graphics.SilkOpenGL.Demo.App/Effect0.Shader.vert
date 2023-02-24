#version 330 core //Using version GLSL version 3.3

layout (location = 0) in vec3 vPos;

uniform mat4 uModel;
        
void main()
{
    gl_Position = uModel * vec4(vPos, 1.0);
}