sampler2D input : register(S0);
float opacity : register(C0);
float4 tint : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 pixel = tex2D(input, uv);
	//return float4(1.0f, 0.0f, 0.0f, 1.0f);
	//return tex2D(input, uv);
	//return color * opacity * tint;
	return float4(tint.rgb, pixel.a);
}