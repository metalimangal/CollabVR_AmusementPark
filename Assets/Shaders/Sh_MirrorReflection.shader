Shader "Custom/Sh_MirrorReflection"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_ReflectionTexLeft("_ReflectionTexLeft", 2D) = "white" {}
		_ReflectionTexRight("_ReflectionTexRight", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID // For Single-Pass Instanced Rendering
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 refl : TEXCOORD1;
				float4 pos : SV_POSITION;

				UNITY_VERTEX_OUTPUT_STEREO // For Single-Pass Instanced Rendering
			};
			float4 _MainTex_ST;
			v2f vert(appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v); // For Single-Pass Instanced Rendering
				UNITY_INITIALIZE_OUTPUT(v2f, o); // For Single-Pass Instanced Rendering
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); // For Single-Pass Instanced Rendering

				o.pos = UnityObjectToClipPos(v.pos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.refl = ComputeScreenPos(o.pos);
				return o;
			}
			sampler2D _MainTex;
			sampler2D _ReflectionTexLeft;
			sampler2D _ReflectionTexRight;
			UNITY_INSTANCING_BUFFER_START(Props)
				// Define instance props here
			UNITY_INSTANCING_BUFFER_END(Props)
			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); // Enables unity_StereoEyeIndex variable

				fixed4 tex = tex2D(_MainTex, i.uv);
				fixed4 refl;
				if (unity_StereoEyeIndex == 0) refl = tex2Dproj(_ReflectionTexLeft, UNITY_PROJ_COORD(i.refl));
				else refl = tex2Dproj(_ReflectionTexRight, UNITY_PROJ_COORD(i.refl));
				return tex * refl;
			}
			ENDCG
		}
	}
}
