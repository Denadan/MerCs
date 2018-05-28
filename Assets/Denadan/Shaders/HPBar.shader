Shader "Denadan/HPBar"
{
	Properties
	{
		[PerRendererData] _MainTex("Full Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[PerRendererData] _Empty("Empty Texture", 2D) = "white" {}
		[PerRendererData] _Tile("Tiling", Int) = (1,0,0,0)
		[PerRendererData] _Value("Camo Color G", Float) = (0,1,0,0)
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
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
			fixed4 color : COLOR;
			float2 texcoord  : TEXCOORD0;
		};

		fixed4 _Color;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = UnityObjectToClipPos(IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color;// *_Color;
	#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
	#endif

			return OUT;
		}

		sampler2D _MainTex;
		sampler2D _AlphaTex;
		sampler2D _Empty;
		float _AlphaSplitEnabled;
		int _Tile;
		float _Value;

		fixed4 SampleSpriteTexture(float2 uv)
		{
			fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
			if (_AlphaSplitEnabled)
				color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

			return color;
		}

		fixed4 frag(v2f IN) : SV_Target
		{
			float2 uv = IN.texcoord;
			uv.x *= _Tile;
			fixed4 f = SampleSpriteTexture(uv) * IN.color;
			fixed4 e = tex2D(_Empty, uv);
			_Value -= IN.texcoord.x;
			_Value = clamp(_Value, 0, 1);
			_Value = sign(_Value);

			f = lerp(e, f, _Value);

			return f;
		}
		ENDCG
	}
	}
}