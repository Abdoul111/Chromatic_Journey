Shader "Custom/GlisteningShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 0, 0, 1)
        _EmissionColor ("Emission Color", Color) = (1, 0, 0, 1)
        _GlistenIntensity ("Glisten Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _EmissionColor;
            float _GlistenIntensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base color
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;

                // Add glistening effect
                float glisten = abs(sin(_Time.y * 2.0)) * _GlistenIntensity;
                col += _EmissionColor * glisten;

                return col;
            }
            ENDCG
        }
    }
}
