Shader "Unlit/ToonPostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightLevels("Light Levels", int) = 5
        _BrightnessOffset("Brightness Offset", float) = 0.2
        _LightSpreadFactor("Light Spread Factor", float) = 0.5
        _Exposure("Exposure", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _LightLevels;
            float _BrightnessOffset;
            float _LightSpreadFactor;
            float _Exposure;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // determine how bright the image is
                float lum = log(Luminance(col) + 1 + _LightSpreadFactor);
                lum *= _LightLevels;
                lum = floor(lum);
            
                // snap color vals to light level
                float lightLevel = lum;
                // return col * (lightLevel/_LightLevels);

                
                return pow(col * (lum/_LightLevels) + _BrightnessOffset, _Exposure);
            }
            ENDCG
        }
    }
}
