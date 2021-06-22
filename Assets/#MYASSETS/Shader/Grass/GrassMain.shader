Shader "MyGrass"
{
    Properties
    {
        _PositionTex("Position Texture", 2D) = "White"{}
        _Intensity("Intensity", Range(1, 50)) = 1
        
        _Color("Colour", Color) = (1,1,1,1)
		_Color2("Colour2", Color) = (1,1,1,1)
		_Width("Width", Float) = 1
		_RandomWidth("Random Width", Float) = 1
		_Height("Height", Float) = 1
		_RandomHeight("Random Height", Float) = 1
		_WindStrength("Wind Strength", Float) = 0.1
		[Space]
		_TessellationUniform("Tessellation Uniform", Range(1, 64)) = 1
    }

    SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}

        Cull Off

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM

                #pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x gles
				#pragma target 5.0

				#pragma require geometry

				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag
				#pragma hull hull
				#pragma domain domain

				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
				#pragma multi_compile _ _SHADOWS_SOFT


                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

                #include "GrassStruct.hlsl"
                #include "CustomTessellation.hlsl"
                #include "Grass.hlsl"
                

                float4 frag(GeometryOutput input) : SV_Target {
					
                    #if SHADOWS_SCREEN
						float4 clipPos = TransformWorldToHClip(input.positionWS);
						float4 shadowCoord = ComputeScreenPos(clipPos);
					#else
						float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
					#endif

					Light mainLight = GetMainLight(shadowCoord);
                    
                    return lerp(_Color, _Color2, input.uv.y) * mainLight.shadowAttenuation;
                }
            ENDHLSL
        }
    }
}