shader_type canvas_item;
uniform sampler2D screen_texture : hint_screen_texture, filter_nearest;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 color = vec4(texture(screen_texture, SCREEN_UV).rgb, texture(TEXTURE, UV).a);
	COLOR = vec4(1.0 - color.rgb, color.a);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
