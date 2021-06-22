Shader "Dissolve"
{
    Properties
    {
        _Color("_Color", Color) = (1, 1, 1, 1)
        [HDR]_DissolveColor("Dissolve", Color) = (1, 1, 1, 1)

        [HDR]_EmissionColor("EmissionColor", Color) = (0, 0, 0, 0)
        _EmissionTexture("EmissionTexture", 2D) = "White"{}

        _Offset("Offset(時間で動かしていますが念のため用意)", float) = 0.0

        _ClipTexture("_ClipTexture", 2D) = "White"{}
        _Cutoff("_AlphaClip", Range(0, 1.11)) = 0   // １だと消え方が不自然なので，少しずらすための0.11を足しています
    }
    SubShader
    {
        Pass{
            HLSLPROGRAM
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        #pragma vertex Vert
        #pragma fragment Frag


        struct Attributes
        {
            float3 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings 
        {
            float2 uv : TEXCOORD0;
            float4 positionCS : SV_POSITION;
        };

        sampler2D _ClipTexture;
        sampler2D _EmissionTexture;

        CBUFFER_START(UnityPerMaterial)
            float4 _ClipTexture_ST;
            float4 _EmissionTexture_ST;
            float _Cutoff;
            float4 _Color;
            float4 _EmissionColor;
            float4 _DissolveColor;
            float _Offset;
        CBUFFER_END


        Varyings Vert(Attributes input)
        {
            Varyings output;

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);
            
            output.positionCS = vertexInput.positionCS;

            output.uv = TRANSFORM_TEX(input.uv, _ClipTexture);


            return output;
        }



        float4 Frag(Varyings input) : SV_TARGET
        {
            float3 color = _Color.rgb;
            float alpha = tex2D(_ClipTexture, input.uv).r;

            float2 emissionTexUV = TRANSFORM_TEX(input.uv, _EmissionTexture);
            emissionTexUV += _Offset + _Time.y/5;
            float3 emission = tex2D(_EmissionTexture, emissionTexUV).rgb;
            float emissionAlpha =  emission.r;
            emission *= _EmissionColor.rgb;

            float clipAlpha = alpha + 0.1 - _Cutoff;
            float s = smoothstep(clipAlpha - 0.01, clipAlpha, 0) - smoothstep(clipAlpha, clipAlpha + 0.01, 0);
            
            float3 dissolve = _DissolveColor * s;
            clip(clipAlpha);

            float3 col = lerp(color, emission, emissionAlpha);

            col = lerp(dissolve, col, clipAlpha);
            return float4(col, 1);
        }
        ENDHLSL
        }
        
    }
}