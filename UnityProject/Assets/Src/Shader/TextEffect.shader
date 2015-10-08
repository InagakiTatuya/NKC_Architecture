Shader "Custom/TextEffect" {
	Properties	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	}

	SubShader	{
		Tags	{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass	{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _OutlineColor;

			v2f vert(appdata_t IN)	{
				v2f OUT;
				OUT.vertex		= mul(UNITY_MATRIX_MVP,IN.vertex);
				OUT.texcoord	= IN.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
				OUT.color		= IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex		= UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}
			
			fixed4 frag(v2f IN) : COLOR	{
				half4	tex	= tex2D(_MainTex,IN.texcoord);
				half4	buf	= _Color * tex.r + _OutlineColor * tex.b;
				buf.a		*= tex.a;
				return	buf;
			}
		ENDCG
		}
	}
}