Shader "SpotShader_BuiltIn"
{
    Properties
    {
        _Color("Color", Color)  = (1,0,0,1)
        _Target("Target", Vector) = (0,0,0,1)
        _TargetIntensity("TargetIntensity", Float) = 100
        _EnvironmentDepth("Environment Depth", 2D) = "white" {} // Add this for the depth texture
    }
    
    SubShader
    {
        LOD 100
        Blend One One
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex    : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
            };

            sampler2D _EnvironmentDepth;
            float4x4 _UnityDisplayTransform;
            float4 _Color;
            float3 _Target;
            float _TargetIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);

                return o;
            }

            float3 GetWorldPosFromUVAndDistance(float2 uv, float distance)
            {
                float4 ndc = float4(2.0 * uv - 1.0, 1, 1); 
                float4 viewDir = mul(unity_CameraInvProjection, ndc);
                #if UNITY_REVERSED_Z
                    viewDir.z = -viewDir.z;
                #endif
                float3 viewPos = viewDir * distance;
                float3 worldPos = mul(unity_CameraToWorld, float4(viewPos, 1)).xyz;
                return worldPos;
            }

            float4 frag (v2f i) : SV_Target
            {
                // UV in screen space
                float2 screenUV = i.screenPosition.xy / i.screenPosition.w;

                // UV in the camera space (using display transform)
                float2 texcoord = mul(float3(screenUV, 1.0f), _UnityDisplayTransform).xy;
                
                // Sample depth texture (R component contains distance info)
                float envDistance = tex2D(_EnvironmentDepth, texcoord).r;

                // Get world position from depth and UVs
                float3 worldPos = GetWorldPosFromUVAndDistance(screenUV, envDistance);

                // Calculate attenuation based on distance to target
                float dist = distance(_Target, worldPos);
                float distanceSqr = max(dot(dist, dist), 0.00001);
                float attenuation = 1.0 / distanceSqr;

                // Return color with attenuation
                return _Color * attenuation / _TargetIntensity;
            }
            ENDCG
        }
    }
}
