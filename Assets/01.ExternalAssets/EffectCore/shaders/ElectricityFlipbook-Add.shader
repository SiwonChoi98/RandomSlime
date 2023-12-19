// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EffectCore/Mobile/Particles/ElectricityFlipbook-Add"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Texture("Texture", 2D) = "white" {}
		_Columns("Columns", Float) = 1
		_Rows("Rows", Float) = 4
		_Speed("Speed", Float) = 1
		_XTiling("XTiling", Float) = 1
		_YTiling("YTiling", Float) = 1
		_ExpMax("ExpMax", Float) = 0
		_ExpMin("ExpMin", Float) = 0
		_Cloud("Cloud", 2D) = "white" {}
		[Toggle(_VERTEXOFFSET_ON)] _VertexOffset("VertexOffset", Float) = 0
		[Toggle(_NOISESINEEXP_ON)] _NoiseSineExp("Noise Sine Exp", Float) = 0
		_MaxNoise("Max-Noise", Float) = 0.8
		_MinNoise("Min-Noise", Float) = -0.8
		_CenterLight("CenterLight", Float) = 0

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend One One
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR
				#pragma shader_feature_local _VERTEXOFFSET_ON
				#pragma shader_feature_local _NOISESINEEXP_ON


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform sampler2D _Cloud;
				uniform float _MinNoise;
				uniform float _MaxNoise;
				uniform sampler2D _Texture;
				uniform float _XTiling;
				uniform float _YTiling;
				uniform float _Columns;
				uniform float _Rows;
				uniform float _Speed;
				uniform float _ExpMax;
				uniform float _ExpMin;
				uniform float _CenterLight;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float4 temp_cast_0 = (0.0).xxxx;
					float2 texCoord70 = v.ase_texcoord1.xy * float2( 0.5,0.5 ) + float2( 0,0 );
					float2 panner69 = ( 1.0 * _Time.y * float2( 0.1,0.5 ) + texCoord70);
					float4 appendResult73 = (float4(0.0 , (_MinNoise + (tex2Dlod( _Cloud, float4( panner69, 0, 0.0) ).g - 0.0) * (_MaxNoise - _MinNoise) / (1.0 - 0.0)) , 0.0 , 0.0));
					#ifdef _VERTEXOFFSET_ON
					float4 staticSwitch80 = appendResult73;
					#else
					float4 staticSwitch80 = temp_cast_0;
					#endif
					

					v.vertex.xyz += staticSwitch80.xyz;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float4 appendResult24 = (float4(_XTiling , _YTiling , 0.0 , 0.0));
					float2 texCoord6 = i.texcoord.xy * appendResult24.xy + float2( 0,0 );
					// *** BEGIN Flipbook UV Animation vars ***
					// Total tiles of Flipbook Texture
					float fbtotaltiles1 = _Columns * _Rows;
					// Offsets for cols and rows of Flipbook Texture
					float fbcolsoffset1 = 1.0f / _Columns;
					float fbrowsoffset1 = 1.0f / _Rows;
					// Speed of animation
					float fbspeed1 = _Time[ 1 ] * _Speed;
					// UV Tiling (col and row offset)
					float2 fbtiling1 = float2(fbcolsoffset1, fbrowsoffset1);
					// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
					// Calculate current tile linear index
					float fbcurrenttileindex1 = round( fmod( fbspeed1 + 0.0, fbtotaltiles1) );
					fbcurrenttileindex1 += ( fbcurrenttileindex1 < 0) ? fbtotaltiles1 : 0;
					// Obtain Offset X coordinate from current tile linear index
					float fblinearindextox1 = round ( fmod ( fbcurrenttileindex1, _Columns ) );
					// Multiply Offset X by coloffset
					float fboffsetx1 = fblinearindextox1 * fbcolsoffset1;
					// Obtain Offset Y coordinate from current tile linear index
					float fblinearindextoy1 = round( fmod( ( fbcurrenttileindex1 - fblinearindextox1 ) / _Columns, _Rows ) );
					// Reverse Y to get tiles from Top to Bottom
					fblinearindextoy1 = (int)(_Rows-1) - fblinearindextoy1;
					// Multiply Offset Y by rowoffset
					float fboffsety1 = fblinearindextoy1 * fbrowsoffset1;
					// UV Offset
					float2 fboffset1 = float2(fboffsetx1, fboffsety1);
					// Flipbook UV
					half2 fbuv1 = texCoord6 * fbtiling1 + fboffset1;
					// *** END Flipbook UV Animation vars ***
					float4 tex2DNode2 = tex2D( _Texture, fbuv1 );
					float4 appendResult91 = (float4(_ExpMin , _ExpMax , 0.0 , 0.0));
					float2 break19_g1 = appendResult91.xy;
					float temp_output_1_0_g1 = _CosTime.w;
					float sinIn7_g1 = sin( temp_output_1_0_g1 );
					float sinInOffset6_g1 = sin( ( temp_output_1_0_g1 + 1.0 ) );
					float lerpResult20_g1 = lerp( break19_g1.x , break19_g1.y , frac( ( sin( ( ( sinIn7_g1 - sinInOffset6_g1 ) * 91.2228 ) ) * 43758.55 ) ));
					#ifdef _NOISESINEEXP_ON
					float staticSwitch84 = ( lerpResult20_g1 + sinIn7_g1 );
					#else
					float staticSwitch84 = _ExpMax;
					#endif
					

					fixed4 col = ( ( i.color * tex2DNode2.r ) + ( pow( tex2DNode2.r , ( i.color.a * staticSwitch84 ) ) * _CenterLight ) );
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	//CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
-1662;156;1014;634;674.4681;4.541689;1.417987;True;False
Node;AmplifyShaderEditor.RangedFloatNode;26;-1353.396,65.01571;Inherit;False;Property;_YTiling;YTiling;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1359.494,-16.23149;Inherit;False;Property;_XTiling;XTiling;4;0;Create;True;0;0;0;False;0;False;1;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-756.1711,778.5437;Inherit;False;Property;_ExpMax;ExpMax;6;0;Create;True;0;0;0;False;0;False;0;24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-747.1011,704.278;Inherit;False;Property;_ExpMin;ExpMin;7;0;Create;True;0;0;0;False;0;False;0;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;91;-481.9011,735.4778;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-1166.696,-1.323173;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CosTime;87;-743.2006,472.878;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1057.272,204.4935;Inherit;False;Property;_Columns;Columns;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1080.702,382.3867;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;1;-10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;85;-518.6274,478.3949;Inherit;False;Noise Sine Wave;-1;;1;a6eff29f739ced848846e3b648af87bd;0;2;1;FLOAT;0;False;2;FLOAT2;15,28;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-974.6459,13.93526;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-1061.677,295.2231;Inherit;False;Property;_Rows;Rows;2;0;Create;True;0;0;0;False;0;False;4;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1220.543,1032.035;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;69;-962.1375,1040.667;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;3;-158.3198,-89.94186;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;1;-761.1333,133.1772;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StaticSwitch;84;-242.5629,646.0286;Inherit;False;Property;_NoiseSineExp;Noise Sine Exp;10;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-463.5758,133.2808;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;0;False;0;False;-1;751f5adbddd79e946bcc6e7490b8a5f2;751f5adbddd79e946bcc6e7490b8a5f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;19.57625,535.6077;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-448.1762,1289.273;Inherit;False;Property;_MaxNoise;Max-Noise;11;0;Create;True;0;0;0;False;0;False;0.8;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;68;-766.1897,1027.447;Inherit;True;Property;_Cloud;Cloud;8;0;Create;True;0;0;0;False;0;False;-1;bcf221e6d872898449ec5e574e958853;751f5adbddd79e946bcc6e7490b8a5f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;-451.7363,1175.534;Inherit;False;Property;_MinNoise;Min-Noise;12;0;Create;True;0;0;0;False;0;False;-0.8;-1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;215.9948,329.3978;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;76;-262.916,1084.299;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.8;False;4;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;313.1809,488.1295;Inherit;False;Property;_CenterLight;CenterLight;13;0;Create;True;0;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-164.2207,1018.024;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-23.94109,1084.906;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;81;47.63981,957.2901;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;261.7655,-46.50827;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;443.548,328.3927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;80;166.8316,1062.301;Inherit;False;Property;_VertexOffset;VertexOffset;9;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;589.8907,88.65202;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;56;926.1401,176.5928;Float;False;True;-1;2;ASEMaterialInspector;0;7;EffectCore/Mobile/Particles/ElectricityFlipbook-Add;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;91;0;90;0
WireConnection;91;1;38;0
WireConnection;24;0;25;0
WireConnection;24;1;26;0
WireConnection;85;1;87;4
WireConnection;85;2;91;0
WireConnection;6;0;24;0
WireConnection;69;0;70;0
WireConnection;1;0;6;0
WireConnection;1;1;9;0
WireConnection;1;2;10;0
WireConnection;1;3;13;0
WireConnection;84;1;38;0
WireConnection;84;0;85;0
WireConnection;2;1;1;0
WireConnection;57;0;3;4
WireConnection;57;1;84;0
WireConnection;68;1;69;0
WireConnection;37;0;2;1
WireConnection;37;1;57;0
WireConnection;76;0;68;2
WireConnection;76;3;82;0
WireConnection;76;4;83;0
WireConnection;73;0;74;0
WireConnection;73;1;76;0
WireConnection;4;0;3;0
WireConnection;4;1;2;1
WireConnection;92;0;37;0
WireConnection;92;1;93;0
WireConnection;80;1;81;0
WireConnection;80;0;73;0
WireConnection;36;0;4;0
WireConnection;36;1;92;0
WireConnection;56;0;36;0
WireConnection;56;1;80;0
ASEEND*/
//CHKSM=6168054214DF8FCCCA644A13D0DBB38A37A2982E