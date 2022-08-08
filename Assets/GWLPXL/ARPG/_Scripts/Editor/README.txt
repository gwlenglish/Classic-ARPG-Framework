ARPG UI by Yashik Moloka for ARPG Attributes, Items, & Abilities (https://assetstore.unity.com/packages/templates/systems/arpg-attributes-items-abilities-169394)

Controls:
LMB(left mouse button) for select tree item
LMB to arrow for expand tree node
LMB + CTRL for multiply select
RMB for delete existing item
LMB to left side of node for drag and drop object


Features:
Good UI
Resizable (for any screen)
Search via FuzzySharp (https://github.com/JakeBayer/FuzzySharp - MIT)
Remember last GameDatabase (via EditorPrefs)
Create any object, delete any object
Save only visited items (optimization)
Fixed ALL memory leaks until working on UI
Smooth tree updates after create/delete item


Project hierarchy:
 - Data - Go to "Extend editor" chapter
 - FuzzySharp - package from github for good search (https://github.com/JakeBayer/FuzzySharp - MIT)
 - EditorModels - only DATA for ObjectEditor to create eazy editor
 - ObjectEditors - Inspector Editors to use it only with ArpgEditorWindow. 
 - ReloadProcessors - For good UX, after creating/deleting items - smooth tree reload
 - YMTree - little package for draw nice tree structure. Keep it independent, i use it for others UI
 - ArpgEditorWindow - main window
 - ArpgItemDataContainer - used as data of tree item
 - YMEditorHelper - just helper


Extend editor with your types:

All you need in Data folder. 
ArpgObjectEditors have mapper for all editor's type for two edit mode - Modify and CreateNew. Just mark for use multiply editors on one type
ArpgTreeData - load data to tree. most types use "ArpgTreeData::DefaultSetupItemPattern". others has hard ui
ArpgStyle - all style stuff


Known issues:

Q. Editor window is too wide when multi-selecting 
A. I don't know what I can do with that. Set ArpgEditorWindow::OnlyFirstSelectedLabelIsDraw to true for draw only first name of selection

Q. Editor not draw properly and console full of errors
A. Try 1) Click to Reload UI button on toolbar 2) Reload DB on toolbar

Q. After import I have error about "'Item' does not contain a definition for 'GetGeneratedItemName'"
A. In new ARPG version GetItemName and SetItemName be replaced by GetGeneratedItemName and SetGeneratedItemName. Just edit it

TODO:
ArpgEditorHelper::GetNameOfArpgObject - check get name from object
Maybe more features