shader_type canvas_item;
render_mode unshaded;
uniform sampler2D screen_texture : hint_screen_texture, filter_nearest;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 color = vec4(texture(screen_texture, SCREEN_UV).rgb, texture(TEXTURE, UV).a);
	COLOR = vec4(1.0 - color.rgb, color.a);
}
