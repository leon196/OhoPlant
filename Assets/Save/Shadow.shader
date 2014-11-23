Shader "Custom/Shadow" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Details ("Details", Range (1.0, 8.0)) = 1.0
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        CGPROGRAM
        #pragma surface surf Lambert

        #include "UnityCG.cginc"
        
        float _Details;

        struct Input {
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
        	float details = floor(_Details);

            o.Emission = float3(0.0, 0.0, 0.0);
            o.Alpha = step(screenUV.y, 0.5) * (1.0 - min(1.0, max(0.0, floor((screenUV.y * 4.0 + 1.0/details) * details) / details)));
        }

        ENDCG
    }
}

