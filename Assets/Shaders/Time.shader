// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Time"
{
	Properties{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color",Color) = (0,0,0,1)
		_Range("Range",range(0,1)) = 0.0
	}
	SubShader
	{ 
		Tags
		{
		"Queue" = "Transparent"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD1;
			};
			sampler2D _MainTex;
			half _Range;
			float4 _Color;
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				//color *= (1-_Range) * _Color;
				color *= lerp((1, 1, 1, 1), _Color, _Range);
				color.a = 1;
				return color;
			}
			ENDCG
		}
	}
}
