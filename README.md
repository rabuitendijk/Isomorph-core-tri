# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

TO BE ADDED:

	- Basic functionallity
		* Allow translation of object independent of coordinate.

	- XML based text importer, competable with microsoft excel xml export.
		* Pass function based arguments "-skip -goto table1 14".
		* Allow for dynamic number of conversation  lines in other languages.
		
	- Map rotation
		* Input rotation filter
		* Object based rotation and Projection of Projection filter.
		* Override directional sprite.


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

	
LIGHTING

	- Properly make a trough wall projection obejct type.
		* Allow color sepecification.
		
	- Create treaded lighting calculation.

	
BUGS:

	- Graphics giltches after zoom that dissappear after camera movement, might be because camera Transform does not snap to pixels.
	
	- BigBlock fisrt Iso in coords[] might be wandering.

	
IN LATER UPDATES:

	- Seclective trasparenty to improve vision.

	- VN like scenes, based on xml input
		* Basic text display and charecter portraits
		* Fading annimation and translation
		* Scaling effects and transitions
		
	- Compatibillity for multiple heights of slab projection (at least [1] and [s2s3]).
	
	- Atlas type batching and draw batching to reduce draw calls.
	
	- True animation