// Source: http://www.codeavarice.com/dev-blog/tutorial-burning-edges-dissolve-shader-in-unity
Shader "Custom/Dissolving" {
	Properties{
	_Color("Color", Color) = (1,1,1,1)
	_MainTex("Albedo (RGB)", 2D) = "white" {}
	[NoScaleOffset]_BumpMap("Normal (RGB)", 2D) = "bump" {}
	_BumpScale("Normal Scale", Range(0,10)) = 1
	_Glossiness("Smoothness", Range(0,1)) = 0.5
	_Metallic("Metallic", Range(0,1)) = 0.0
	[NoScaleOffset]_MetallicGlossMap("Metallic Map", 2D) = "white" {}
	[NoScaleOffset] _EmissionMap("Emission Map", 2D) = "black" {}
	[HDR]_Emission("Emission", Color) = (0, 0, 0, 0)

	_SliceAmount("Dissolve Amount", Range(0.0, 1.0)) = 0.5
	[NoScaleOffset]_SliceGuide("Dissolve Texture", 2D) = "white" {}

	_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.03
	[HDR]_BurnColor("BurnColor", Color) = (1,1,1,1)
	[NoScaleOffset]_BurnRamp("Burn Ramp", 2D) = "white" {}
	}
		SubShader{
		  Tags { "RenderType" = "Opaque" }
		  LOD 200

		CGPROGRAM
		//if you're not planning on using shadows, remove "addshadow" for better performance
		#pragma surface surf Standard addshadow 
		#pragma target 3.0
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _MetallicGlossMap;
		sampler2D _EmissionMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SliceGuide;
			float _SliceAmount;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _Emission;

		sampler2D _SliceGuide;
		float _SliceAmount;
		sampler2D _BurnRamp;
		float _BurnSize;
		float4 _BurnColor;
		float _BumpScale;

	  void surf(Input IN, inout SurfaceOutputStandard o) {

		  clip(tex2D(_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);
		  // Metallic and smoothness come from slider variables
		  fixed4 metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex);
		  o.Metallic = _Metallic * metallic.rgb;
		  o.Smoothness = _Glossiness * metallic.a;
		  // color and normal
		  fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color;
		  fixed3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
		  o.Albedo = c.rgb;
		  o.Normal = normal;
		  // emission
		  fixed4 em = tex2D(_EmissionMap, IN.uv_MainTex)*_Emission;
		  o.Emission = em;
		  half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - _SliceAmount;
		  // burn edges
		  if (test < _BurnSize) {//&& _SliceAmount > 0 && _SliceAmount < 1) {
			o.Emission = tex2D(_BurnRamp, float2(test *(1 / _BurnSize), 0)) * _BurnColor;
			o.Albedo *= o.Emission;
		  }
		  o.Alpha = c.a;
		}
		  ENDCG
	}
		Fallback "Diffuse"
}