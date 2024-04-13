shader_type canvas_item;

uniform vec3 outlineColor : source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	COLOR = texture(TEXTURE, UV); //read from texture
	if (COLOR.a < 0.1)
	{
		COLOR.rgb = outlineColor;
		vec4 check = texture(TEXTURE, UV + vec2(TEXTURE_PIXEL_SIZE.x, 0));
		vec4 check2 = texture(TEXTURE, UV + vec2(0, TEXTURE_PIXEL_SIZE.y));
		vec4 check3 = texture(TEXTURE, UV - vec2(TEXTURE_PIXEL_SIZE.x, 0));
		vec4 check4 = texture(TEXTURE, UV - vec2(0, TEXTURE_PIXEL_SIZE.y));
		COLOR.a = max(check.a, max(check2.a, max(check3.a, check4.a)));
	}
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
