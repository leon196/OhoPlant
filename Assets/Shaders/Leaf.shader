Shader "Custom/Leaf" {
	Properties {
		_ColorA ("Color A", Color) = (1,1,1,1)
		_ColorB ("Color B", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert
		#include "UnityCG.cginc"

		struct Input 
		{
			float2 uv_MainTex;
			float4 screenPos;
          	float3 worldPos;
		};

		fixed4 _ColorA;
		fixed4 _ColorB;
		uniform float _PlantHeight;

		void surf (Input IN, inout SurfaceOutput o)
		{
			//float3 screenUV = IN.screenPos.xyz / IN.screenPos.w;

			float shadow = sin(IN.uv_MainTex.x * 3.1416) + sin(IN.uv_MainTex.y * 3.1416);

			fixed4 color = lerp(_ColorA, _ColorB, shadow * 0.5);

			o.Emission = color.rgb;
			o.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Unlit/Transparent"
}


