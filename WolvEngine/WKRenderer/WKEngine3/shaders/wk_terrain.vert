#version 460

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec2 inUV;
layout (location = 3) in vec4 inColor;

layout (set = 0, binding = 0) uniform UBO {
	mat4 projection;
	mat4 view;
	mat4 model;
} ubo;

layout (set = 1, binding = 0) uniform Node {
	mat4 matrix;
} node;

layout (location = 0) out vec3 outNormal;
layout (location = 1) out vec3 outColor;
layout (location = 2) out vec3 outViewVec;
layout (location = 3) out vec3 outLightVec;

out gl_PerVertex
{
	vec4 gl_Position;
};

void main() 
{	
	vec4 pos = vec4(inPos, 1.0);

	gl_Position = ubo.projection * ubo.view * node.matrix * pos;
	outNormal = mat3(ubo.view * node.matrix) * inNormal;
	vec4 localpos = ubo.view * node.matrix * pos;

	vec3 lightPos = vec3(0.0f, 1000000.0f, 10.0f);
	outLightVec = lightPos.xyz - localpos.xyz;
	outViewVec = -localpos.xyz;		

	outColor = vec3(inPos.z,inColor.yz);

}
