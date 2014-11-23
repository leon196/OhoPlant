Shader "Custom/Sky" {
    Properties {
      	_MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,0)
        _Details ("Details", Range (1.0, 128.0)) = 32.0
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
        uniform float _Details;
        
        uniform float WorldTime;
        uniform float WorldSpeed;
        uniform float WorldLight;

        uniform float slider1;
        uniform float slider2;

        uniform float spiner1;
        uniform float spiner2;
        uniform float spiner3;

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

		void surf (Input IN, inout SurfaceOutput o) {
			
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			

        	float details = floor(_Details);

        	float time = WorldTime * WorldSpeed;
        	float2 animDirection = float2(0.0, -time);
        	float2 uv = IN.uv_MainTex;
        	
        	//uv.y = uv.y * 0.5 * (1.0 + cos(time * 2.0 + screenUV.x));
        	//uv.y * (screenUV.x + cos(screenUV.x * 10.0) * 0.1) - screenUV.y * 0.5;

        	uv = pixelize(uv + float2(0.0, 0.1 * floor(cos(uv.x * uv.x + time * 10.0) * 16.0) / 16.0), details);

            //uv.y += time * 0.000001;
        	float cloud = step(rand(uv.yy), 0.5) * rand(uv.yy);

            float horizon = (1.0 - screenUV.y + 0.5) * 0.25;

        	float3 color = _Color.rgb + cloud;// + horizon;

			o.Alpha = 1.0;
			o.Emission = color * WorldLight;
		}

        ENDCG
    }
}

