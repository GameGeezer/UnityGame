Shader "Custom/TestMaterialShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader{

			Pass {
				Tags{ "LightMode" = "ForwardBase" }
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				uniform float4 _Color;
				uniform float4 _LightColor0;

				struct VertexInput {
					float4 position : POSITION;
					float3 normal : NORMAL;
					float4 color : COLOR;
				};
				struct VertexOutput {
					float4 position : SV_POSITION;
					float4 color : COLOR;
				};
				VertexOutput vert(VertexInput v) {
					VertexOutput o;

					float3 normalDirection = normalize(mul(float4(v.normal, 0.0f), _World2Object).xyz);

					float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

					float lambertCoeff = max(dot(normalDirection, lightDirection), 0.0f);
					float atten = 1.0f;

					float4 color = _LightColor0 * _Color * v.color * lambertCoeff;

					o.color = color;
					o.position = mul(UNITY_MATRIX_MVP, v.position);

					return o;
				}

				float4 frag(VertexOutput i) : COLOR {
					return i.color;
				}
				ENDCG
		}
		
	}
	FallBack "Diffuse"
}
