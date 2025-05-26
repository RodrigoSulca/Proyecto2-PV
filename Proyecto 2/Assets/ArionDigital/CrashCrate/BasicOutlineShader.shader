Shader "Custom/TexturedOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _Outline ("Outline width", Float) = 0.03
    }
    SubShader
    {
        Tags {"Queue"="Overlay"}
        Cull Back

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        fixed4 _OutlineColor;
        float _Outline;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
        }
        ENDCG

        // Outline pass
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            Offset 15,15

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _Outline;
            uniform float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float4 pos = UnityObjectToClipPos(v.vertex);
                pos.xy += norm.xy * _Outline * pos.w;
                o.pos = pos;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
