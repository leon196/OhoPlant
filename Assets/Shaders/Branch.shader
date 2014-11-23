Shader "Custom/Branch" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Ground Color", Color) = (1,1,1,0)
        _Details ("Details", Range (1.0, 8.0)) = 1.0
        _Shades ("Shades", Range (0.0, 1.0)) = 0.2
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        CGPROGRAM
        #pragma surface surf Lambert

        #include "UnityCG.cginc"
        
        uniform float4 _Color;
        uniform float4 _ColorGrass;
        uniform float _Details;
        uniform float _Shades;

        uniform float WorldTime;
        uniform float WorldSpeed;
        uniform float WorldLight;
        uniform float WorldDetails;
        uniform float4 MoonDirection;

        float rand(float2 co){
            return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
        }

        float2 pixelize(float2 uv, float details) {
        	return floor(uv.xy * details) / details;
        }

        struct Input {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) 
        {
        	// Level of Details
        	float details = floor(WorldDetails);//pow(2, floor(_Details));
            
            // Screen UVs
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            screenUV.x *= _ScreenParams.x / _ScreenParams.y;
        	screenUV = pixelize(screenUV.xy, details);
        	screenUV.x = 1.0 - screenUV.x;

        	// Mesh UVs
        	//float2 uv = pixelize(IN.uv_MainTex.xy, details);

            // Color & Alpha init
        	float3 color = _Color.rgb;
        	float alpha = 1.0;

            // Time & Animation
        	float time = WorldTime * WorldSpeed;
        	MoonDirection.y = MoonDirection.y * -1.0;
        	MoonDirection.xy = pixelize(MoonDirection.xy, 4.0);
        	float2 anim = pixelize(MoonDirection.xy * time, details);

            // Random & Shades
            float random = rand(screenUV.xy + anim);
            float shade = min(1.0, random + _Shades);

            // Return
            o.Emission = color * shade;
            o.Alpha = alpha;
        }

        ENDCG
    }
}

