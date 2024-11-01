// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthGrayscale" {
SubShader {
Tags { "RenderType"="Opaque" }

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _CameraDepthTexture;

struct v2f {
float4 pos : POSITION;
float4 scrPos:TEXCOORD1;
};

//Vertex Shader
v2f vert (appdata_base v){
v2f o;
o.pos = UnityObjectToClipPos (v.vertex);
o.scrPos=ComputeScreenPos(o.pos);
//for some reason, the y position of the depth texture comes out inverted
o.scrPos.y = o.scrPos.y;
return o;
}

//Fragment Shader
half4 frag (v2f i) : COLOR{
float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
half4 depth;

depth.r = depthValue;
depth.g = depthValue;
depth.b = depthValue;

depth.a = 1;
return depth;
}
ENDCG
}
}
FallBack "Diffuse"
}

// there still so many things i don't understand like
// you "tells the camera to render depth information" like where is it rendered to?
// How exactly does OnRenderImage works like where is the source and destination RenderTexture gotten from

// what Vert is the vertShader called on since we did't apply the material containing the shader to a gameobject
// what is sampler2D what does it do exactly