# HomaTool


Personal Notes
- I try to simplify the process as much as possible with only a button.
- Item IDs and prices have a formula related to each other, similar to existing items.
- Prefab component settings like animator contollers and collider sizes are built similar to existing items.
- The more 3D models and 2D renders are alike each other (such as rig&bone count, texture size of POT, etc), the more optimization can be possible.
- There are 2 editor scripts with comments for each and every step.
- Please feel free to contact for anything.


Instructions
1) Make sure you have put all your 3D .fbx models in Assets/Resources/Models
2) Make sure you have put all your 2D .png renders in Assets/Resources/Sprites
3) Make sure all of the related files of a new store item has the same name.
4) Type this name to the input field below and hit the button!

Example: If you want to add a new shop item named "NewShopItem", you must have "NewShopItem.fbx" and "NewShopItem.png" in above paths. 
Then, type "NewShopItem" to the input field below and enjoy!


Important Notes
- Every file corresponding to the same item MUST have the same name.
- Files MUST be in the paths listed above.
- Adding single or multiple items together is possible. Use comma spearated item names without any space in between.
- Potentially missing files are files that are usually correctly located and named but forgotten to enter as an input.


What can be improved?
- To use different items on different paths, drag and drop for folder locations can be implemented.(However it lengthens the automation process and might create human errors)
- Optimizations can go furher depending on the mutual characteristics of 3D and 2D items.
