# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

PLANNING:

	- Build a controller mangager for switching between scenes.
	- Build base controller for translation testing.
	- Impliment a translational type, boundry boxes, depth updates and stuff.
	- Implement unbound translation
	
	- Build collision controller
		* return event stack based on collision box, velocity and direction. [static collision]
		* process event stack to return new postion.
		* add z-axis movement
		* stairs
		* test nonstatic translatible objects, depthchecks and stuff.
		* impliment non-static collision
	
	- Update asset loading to v3
		* Many added tags for collision, lighting and color.
		{
			bool editor_only
			float light_r
			float light_g
			float light_b
			bool blocks_lighting
			bool dir_lighting
			bool no_collider
			bool staircase
			bool block_view //For possible optimisation later
			string tileset
		}
		* Concider true annimations in implementaion.
		* Dense texture atlas.
		* Move UnitDefinition to hidden folder
		* Add save to spesific tileset feature
		* Translatable xml object type?
	
	- Update lighting to v2
		* directional lingthing.
		* flood update on rotation, by queue?
		* lighting color.
		* exclude by tag from field or directional lighting.
		* stop light from going trough walls.
		* sqrt based light level correction.
		* get translatable light source.
		* allow translatable object to draw light level from near tiles.

TO BE ADDED:

	- XML based text importer, competable with microsoft excel xml export.
		* Pass function based arguments "-skip -goto table1 14".
		* Allow for dynamic number of conversation  lines in other languages.		


MAP EDITOR:
		
	- Saving/Loading
		* Devide referential and non-referential saving files.


TRANSLATION:

	- Create translatable object type.
		
	- Collsion checking
		* Object static collision.
		* Object object collision.
	
	- Z axis movement.
	
	- Add object interactebillity tags to XML.

	
BUGS:

	- Graphics giltches after zoom that dissappear after camera movement, might be because camera Transform does not snap to pixels.
	
	- BigBlock fisrt Iso in coords[] might be wandering.
	
	- In map editor the mouse hover gost sprite does not properly update its sprite when map rotatating.

	- Atlas padding is to large causing 14 instead of 15 tiles per row
	
IN LATER UPDATES:

	- Seclective trasparenty to improve vision.

	- VN like scenes, based on xml input
		* Basic text display and charecter portraits.
		* Fading annimation and translation.
		* Scaling effects and transitions.
	
	- Atlas type batching and draw batching to reduce draw calls.
	
	- True animation.
	
	- Colored ligthing.
	
	- Lighting system tags.
	
	- Collision system tags.
	
	- Tight atlas packing using hex meshes.
