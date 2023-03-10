-- Vertex
#version 150
// Copyright (c) 2008 the OpenTK Team. See license.txt for legal bla

// custom vertex attribute
in vec3 InPosition;
in vec3 InNormal;
in vec3 InTangent;
in vec2 InTexCoord;

// transformation matrix
uniform mat4 ModelViewMatrix;
uniform mat4 ModelViewProjectionMatrix;
uniform mat3 NormalMatrix;

// world uniforms
uniform vec3 Light_Position;
uniform vec3 Camera_Position;

// MUST be written to for FS
out vec2 TexCoord;
out vec3 VaryingLightVector;
out vec3 VaryingEyeVector;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	TexCoord = InTexCoord;

	vec3 nor = normalize(NormalMatrix * InNormal);
	vec3 tan = normalize(NormalMatrix * InTangent);
	vec3 bi = cross(nor, tan);
  
	// need positions in tangent space
	vec3 vertex = vec3(ModelViewMatrix * vec4(InPosition,1));

	vec3 temp = Light_Position - vertex;
	VaryingLightVector.x = dot(temp, tan); // optimization, calculate dot products rather than building TBN matrix
	VaryingLightVector.y = dot(temp, bi);
	VaryingLightVector.z = dot(temp, nor);

	temp = Camera_Position - vertex;
	VaryingEyeVector.x = dot(temp, tan);
	VaryingEyeVector.y = dot(temp, bi);
	VaryingEyeVector.z = dot(temp, nor);
}

-- Fragment
#version 150
// Copyright (c) 2008 the OpenTK Team. See license.txt for legal bla

// Material uniforms
uniform sampler2D Material_DiffuseAndHeight;
uniform sampler2D Material_NormalAndGloss;
uniform vec3 Material_ScaleBiasShininess; // x=Scale, y=Bias, z=Shininess

// Light uniforms
uniform vec3 Light_DiffuseColor;
uniform vec3 Light_SpecularColor;

// from VS
in vec2 TexCoord;
in vec3 VaryingLightVector;
in vec3 VaryingEyeVector;

out vec4 FragColor;

vec3 normal;

void main()
{ 
	vec3 lightVector = normalize( VaryingLightVector );
	vec3 eyeVector = normalize( VaryingEyeVector );

	// first, find the parallax displacement by reading only the height map
	float parallaxOffset = texture2D( Material_DiffuseAndHeight, TexCoord.st ).a *
		Material_ScaleBiasShininess.x - Material_ScaleBiasShininess.y;
	// displace texcoords according to viewer
	vec2 newTexCoords = TexCoord.st + ( parallaxOffset * eyeVector.xy );

	// knowing the displacement, read RGB, Normal and Gloss
	vec3 diffuseColor = texture2D( Material_DiffuseAndHeight, newTexCoords.st ).rgb;
	vec4 temp = texture2D( Material_NormalAndGloss, newTexCoords.st );
  
	// build a usable normal vector
	normal.xy = temp.ag * 2.0 - 1.0; // swizzle alpha and green to x/y and scale to [-1..+1]
	normal.z = sqrt( 1.0 - normal.x*normal.x - normal.y*normal.y ); // z = sqrt(1-x^2-y^2)
  
	// move other properties to be better readable
	float gloss = temp.r;
  
	//  float alpha = temp.b;
	//  if ( alpha < 0.2 ) // optimization: should move this test before reading RGB texture
	//    discard;
  
	// tweaked phong lighting
	float lambert = max( dot( lightVector, normal ), 0.0 );

	FragColor = vec4( Light_DiffuseColor * diffuseColor, 1.0 ) *
	lambert;

	if ( lambert > 0.0 )
	{
		float specular = pow(clamp(dot(reflect(-lightVector, normal), eyeVector), 0.0, 1.0),
			Material_ScaleBiasShininess.z );

		FragColor += vec4( Light_SpecularColor * diffuseColor, 1.0 ) * ( specular * gloss );
	}
}