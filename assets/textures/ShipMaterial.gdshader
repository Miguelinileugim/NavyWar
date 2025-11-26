shader_type canvas_item;
uniform sampler2D texture_albedo; //

void fragment() {
    // Sample the texture and apply it to the fragment.
    COLOR = texture(texture_albedo, UV);
}