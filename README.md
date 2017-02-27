# Isomorph-core-tri
Isomorph, Third iteration, Unity 5, Flat sprite based approach. Goals: proper sorting algorithm, unit sprite slicing and clean sprite finishing.

ESSENTIAL: gain understanding how projection to screen and object depth and coordinates relate.

To be added
	- Basic functionallity
		{alpha-1}* Iso classes 
		* Map storage, concider normal x by x by x grid storage. Map class with main return Map, height witdh and depth
		{alpha-1}* Ingame object builder
	[Under concideration]- Compatibillity for multiple heights of slab projection (at least [1] and [srt2_srt3]).
	- XML based text importer, competable with microsoft excel xml export.
	[sOLVED MABY?]- A way to sort the sprites without using to lagre a numeric width.
	- A mouse click even collision checker using math and stuff.
		* find a way to obtain all coordinates object in a  simulated "raycast" in order.
		* wirte a scripts that efficiently goes trough them to find first collision.
		* Find appropriate selectable object on vertical stack.
	- Multi part object loader
	- Object Rotation script

Methods [Not nesseseraly in engine] that have to be disscovered
	- Proper sprite slicing
		*Current concideration png++ c++ program that loads pixel-perfect refence and slice image to create sliced file.

To be updated


For later updates
	- Map edditor
		* Saving/Loading
	- Map rotation.
	- Seclective trasparenty to imporve vision.

Currect ok state
	- Basic functionallity
		{alpha-1}* Iso classes 
		{alpha-1}* Ingame object builder
	- How to clean to graphics edges
		*Pogress has been made by using 15x compession from source.
		*Pixel by pixel art cleaning, Unity source file sould have 1 or 0 aplha pixels otherwise it causes smudging.
		*A guide has been consruced at decen accuraty, considering increasing asset size from 264px to 524px. 
