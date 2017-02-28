# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

ESSENTIAL: 

	- Gain understanding how projection to screen and object depth and coordinates relate.
	- 264 is not a multiple of 2, 256 is...

TO BE ADDED:

	- Basic functionallity
		* Functioning hover selector and mouse click Tile return
		* Allow translation of object independent of coordinate.

	- [Under concideration] Compatibillity for multiple heights of slab projection (at least [1] and [srt2_srt3]).

	- XML based text importer, competable with microsoft excel xml export.

	- A mouse click even collision checker using math and stuff.
		* find a way to obtain all coordinates object in a  simulated "raycast" in order.
		* wirte a scripts that efficiently goes trough them to find first collision.
		* Find appropriate selectable object on vertical stack.

	- Proper sprite slicing
		* Sprite slicing will be hendland before XMLSpriteLoader call building sprites into a custem directionairy (for rule splitting)
		* 32bit RGBA import and export.
		* Automatic xml file generation for XMLSpriteLoader.


TO BE UPDATED:

	- Multi part object loader
		* Extend XMLSpriteLoader to generate an multi obeject type

	- Object Rotation script
		* Extend XMLSpriteLoader to generate an obeject type


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


IN OK STATE:

	- Basic functionallity
		* {alpha-2} Iso classes 
		* {apha-1} Map storage, concider normal x by x by x grid storage. Map class with main return Map, height witdh and depth
		* {alpha-1} Ingame object builder.

	- [May be solved?] A way to sort the sprites without using to lagre a numeric width.

	- How to clean to graphics edges
		* Pogress has been made by using 15x compession from source.
		* Pixel by pixel art cleaning, Unity source file sould have 1 or 0 aplha pixels otherwise it causes smudging.
		* A guide has been consruced at decen accuraty, considering increasing asset size from 264px to 524px. 
