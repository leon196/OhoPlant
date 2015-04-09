Shader "Custom/Circle" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
   		Pass {
		    Cull off
	    	Blend SrcAlpha OneMinusSrcAlpha     
		    ZWrite Off
			LOD 200
			
			CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag   
	    	#include "UnityCG.cginc"   

		    struct v2f {
		        float4  pos : SV_POSITION;
		        float2  uv : TEXCOORD0;
		    };   

			sampler2D _MainTex;
	    	float4 _MainTex_ST; 
			fixed4 _Color;

			v2f vert (appdata_base v)
			{
		        v2f o;
		        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		        o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
		        return o;
		    }   

		    half4 frag (v2f i) : COLOR
		    {     
				fixed4 color = _Color;

				float radius = distance(float2(0.5, 0.5), i.uv.xy);

				color.rgb *= (1.0 - smoothstep(0.25, 0.35, radius)) * 0.5 + 0.5;
				color.a = 1.0 - smoothstep(0.25, 0.5, radius);

		        return color;
		    }
			ENDCG
		} 
	}
	FallBack "Unlit/Transparent"
}
