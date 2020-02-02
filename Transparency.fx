sampler2D input : register(S0);
float opacity : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	//return float4(1.0f, 0.0f, 0.0f, 1.0f);
	return tex2D(input, uv);
	//float4 color = tex2D(input, uv);
	//return color * opacity;
}