# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

TO BE ADDED:

	- Basic functionallity
		* Functioning hover selector and mouse click Tile return
		* Allow translation of object independent of coordinate.

	- XML based text importer, competable with microsoft excel xml export.

	- A mouse click even collision checker using math and stuff.
		* Find appropriate selectable object on vertical stack.

		
	- Object Rotation script
		* Extend XMLSpriteLoader to generate an obeject type


BUGS:

	- Graphics giltches after zoom that dissabpear after camera movement, might be because camera Transform does not snap to pixels.


IN LATER UPDATES:

	- Map edditor
		* Saving/Loading

	- Map rotation.

	- Seclective trasparenty to improve vision.

	- Translation annimantion

	- Simulated lighting levels (By material coloring)

	- VN like scenes, based on xml input
		* Basic text display and carecter portraits
		* Fading annimation and translation
		* Scaling effects and transitions
		
	- Compatibillity for multiple heights of slab projection (at least [1] and [s2s3]).


IN OK STATE:

	- Basic functionallity
		* {alpha-2} Iso classes 
		* {apha-1} Map storage, concider normal x by x by x grid storage. Map class with main return Map, height witdh and depth
		* {alpha-1} Ingame object builder.
		* Half tile support added.
		
	- Proper sprite slicing
		* Generate objects from xml containing data binding sprites together.
		* Autogenerate object xml for alias.
		* Sprite slicing will be hendland before XMLSpriteLoader call building sprites into a custem directionairy (for rule splitting)
		* 32bit RGBA import and export.
		* Automatic xml file generation for XMLSpriteLoader.
		* Mip level overides for below standard resolution zooming.
		
	- A mouse click even collision checker using math and stuff.
		* find a way to obtain all coordinates object in a  simulated "raycast" in order.
		* wirte a scripts that efficiently goes trough them to find first collision.
		
	- [May be solved?] A way to sort the sprites without using to lagre a numeric width.

	- How to clean to graphics edges
		* Pogress has been made by using 15x compession from source.
		* Pixel by pixel art cleaning, Unity source file sould have 1 or 0 aplha pixels otherwise it causes smudging.
		* A guide has been consruced at decen accuraty, considering increasing asset size from 264px to 524px. 
