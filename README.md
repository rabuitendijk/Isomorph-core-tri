# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

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
