extends MeshInstance3D


func _ready():
	# Create a new SurfaceTool to build the mesh.
	var st = SurfaceTool.new()
	st.begin(Mesh.PRIMITIVE_TRIANGLES)
	
	# Shear amount: adjust this value to control the skew of the top face.
	var shear = 0.5
	
	# Define vertices for a unit block.
	# Bottom face (remains unchanged)
	var v0 = Vector3(0, 0, 0)
	var v1 = Vector3(1, 0, 0)
	var v2 = Vector3(1, 0, 1)
	var v3 = Vector3(0, 0, 1)
	
	# Top face (sheared along the X-axis by the 'shear' value)
	var v4 = Vector3(shear, 1, 0)
	var v5 = Vector3(1 + shear, 1, 0)
	var v6 = Vector3(1 + shear, 1, 1)
	var v7 = Vector3(shear, 1, 1)
	
	# Bottom face (two triangles)
	st.add_vertex(v0)
	st.add_vertex(v1)
	st.add_vertex(v2)
	
	st.add_vertex(v0)
	st.add_vertex(v2)
	st.add_vertex(v3)
	
	# Top face (two triangles)
	st.add_vertex(v4)
	st.add_vertex(v6)
	st.add_vertex(v5)
	
	st.add_vertex(v4)
	st.add_vertex(v7)
	st.add_vertex(v6)
	
	# Front face (v0, v1, v5, v4)
	st.add_vertex(v0)
	st.add_vertex(v1)
	st.add_vertex(v5)
	
	st.add_vertex(v0)
	st.add_vertex(v5)
	st.add_vertex(v4)
	
	# Right face (v1, v2, v6, v5)
	st.add_vertex(v1)
	st.add_vertex(v2)
	st.add_vertex(v6)
	
	st.add_vertex(v1)
	st.add_vertex(v6)
	st.add_vertex(v5)
	
	# Back face (v2, v3, v7, v6)
	st.add_vertex(v2)
	st.add_vertex(v3)
	st.add_vertex(v7)
	
	st.add_vertex(v2)
	st.add_vertex(v7)
	st.add_vertex(v6)
	
	# Left face (v3, v0, v4, v7)
	st.add_vertex(v3)
	st.add_vertex(v0)
	st.add_vertex(v4)
	
	st.add_vertex(v3)
	st.add_vertex(v4)
	st.add_vertex(v7)
	
	# Generate normals for correct lighting.
	st.generate_normals()
	
	# Commit the mesh and assign it to this MeshInstance.
	var mesh = st.commit()
	self.mesh = mesh
