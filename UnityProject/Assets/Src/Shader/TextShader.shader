Shader "Custom/TextShader" {
	Properties	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
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
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)	{
				v2f OUT;
				OUT.vertex		= mul(UNITY_MATRIX_MVP,IN.vertex);
				OUT.texcoord	= IN.texcoord;
				OUT.color		= IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex		= UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : COLOR	{
				half4	sum	= tex2D(_MainTex,IN.texcoord) * IN.color;
				sum	+= tex2D(_MainTex,IN.texcoord + half2( 0.0001, 0.0f))  * IN.color;
				sum	+= tex2D(_MainTex,IN.texcoord + half2( 0.00, 0.0001f)) * IN.color;
				sum	+= tex2D(_MainTex,IN.texcoord + half2(-0.0001, 0.0f))  * IN.color;
				sum	+= tex2D(_MainTex,IN.texcoord + half2( 0.00,-0.0001f)) * IN.color;
				sum /= 5.0f;
				if(sum.a > 0.5f)	return	half4(0.0f,0.75f,0.75f,1.0f);
				if(sum.a > 0.0f)	return	half4(1.0f,1.0f,1.0f,pow(sum.a * 2.0f,0.15f));
				return	half4(1.0f,1.0f,1.0f,0.0f);
			}
		ENDCG
		}
	}
}