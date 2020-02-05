sampler2D input : register(S0);
float saturation : register(C0);
float lightness : register(C1);
float value : register(C2);
float4 tint : register(C3);

// lerp(min, max, alpha)

float4 blend3(float4 left, float4 mid, float4 right, float pos)
{
	if (pos < 0) return lerp(left, mid, pos + 1);
	if (pos > 0) return lerp(mid, right, pos);
	return mid;
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 pixel = tex2D(input, uv);
	float4 black = float4(0, 0, 0, pixel.a);
	float4 white = float4(1, 1, 1, pixel.a);
	if (lightness <= -1) return black;
	if (lightness > 1) return white;

	float4 color = float4(lerp(float3(0.5f, 0.5f, 0.5f), tint.rgb, saturation), pixel.a);

	if (lightness >= 0) 
		return float4(blend3(black, color, white, 2.0f * (1.0f - lightness) * (value - 1) + 1).rgb, pixel.a);

	return float4(blend3(black, color, white, 2.0f * (1.0f + lightness) * (value) - 1).rgb, pixel.a);
}