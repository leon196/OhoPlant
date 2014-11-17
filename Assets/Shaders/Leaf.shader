﻿Shader "Custom/Leaf" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0)
        _Curve ("Curve", Range(0.0, 1.0)) = 0.5
        _Shade ("Shade", Range(0.0, 1.0)) = 0.5
        _Details ("Details", Range (1.0, 64.0)) = 1.0
        _Radius ("Radius", Range (0.0, 1.0)) = 0.5
	}
	SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

        #include "UnityCG.cginc"

		sampler2D _MainTex;
        float4 _Color;
        float _Curve;
        float _Shade;
        float _Details;
        float _Radius;

        float rand(float2 co){
            return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
        }

        float2 pixelize(float2 uv, float details) {
        	return floor(uv.xy * details) / details;
        }

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {

			float details = floor(_Details);
			float2 uv = IN.uv_MainTex.xy;
			//uv.y = 1.0 - uv.y;
			uv = pixelize(uv, details);
			uv.x += _Curve * 0.5;
			uv.x /= 1.0 + _Curve;
			uv.y += (1.0 - _Curve) * 0.5;
			uv.y /= 1.0 + (1.0 - _Curve);
			//uv.x += _Curve;
            //float border = 1.0 - (1.0 + cos(uv.x * 6.28318530718)) * 0.5;
            //uv.y += border * _Curve;
            //uv.y *= 2.0 * _Curve;
            //uv.y -= 1.0 * _Curve;
			float shade = min(1.0, rand(uv.xy) + _Shade);
			half3 color = _Color.rgb * shade;
			float alpha = step(distance(uv, float2(0.5, 0.5)), _Radius);

			o.Emission = color;
			o.Alpha = alpha;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}