**********		유니티 슬라이딩 퍼즐 에셋 솔루션	**********

1.개요
	이 에셋은 슬라이딩 퍼즐 솔루션으로 오브젝트들로만 이루어져 있으며 UI요소는 없습니다.
	이 에셋은 기본적으로 유니티 3D 프로젝트에서 유효합니다.

2.사용법
	※ 이 에셋을 사용하기 위해서는 필수적으로 레이어 추가가 필요합니다.
	    - 20: SlidingPuzzle (=슬라이딩 퍼즐 동작을 기본 레이어)
		- 31: StartBtn (=Demo 씬에서의 활성화 버튼을 위한 테스트 레이어)
	    - 자세한 레이어는 "PuzzleTagManager.preset" 파일을 통해 확인하세요.

	프리팹
		-Preset1, Preset2, Preset3를 해당 씬에다가 배치하여 사용할 수 있습니다.
		해당 오브젝트는 모두다
		Preset
			SlidingPuzzle
				-Background
				-BoardBackground
					-Board
			Button
				SlidingPuzleExitButton
					Text
				SlidingPuzleShuffleButton
					Text
		의 구조를 하고 있습니다.
		-Background
			퍼즐의 배경이며 해당 이미지는 ImageController에서 수정이 가능합니다.
		-BoardBackground
			보드의 배경을 나타내며 해당 이미지는 ImageController에서 수정이 가능합니다.
		-Board
			실질적인 보드게임의 오브젝트이며 조작이 가능합니다.
			Board의 Board에서 Size를 통해 2 ~ 8까지의  N X N퍼즐을 생성할 수 있습니다.
			ImageController에서 타일에 적용될 이미지를 지정할 수 있습니다.
		-SlidingPuzleExitButton
			퍼즐 게임을 종료하는 버튼입니다.
			아래의 자식 오브젝트 Text에서 원하는 텍스트를 넣을 수 있습니다.
			ImageController에서 해당 오브젝트의 이미지를 지정할 수 있습니다.
		-SlidingPuzleShuffleButton
			퍼즐을 섞는 함수입니다.
			아래의 자식 오브젝트 Text에서 원하는 텍스트를 넣을 수 있습니다.
			ImageController에서 해당 오브젝트의 이미지를 지정할 수 있습니다.
	
	스크립트
		-BoardList
			씬에 빈 프로젝트를 배치한 뒤 해당 스크립트를 적용할 것을 권장합니다.
			씬에 배치된 퍼즐을 관리하는 함수입니다.
			Boards의 리스트엔 Element로 Board오브젝트와 Button 오브젝트가 있습니다.
			Board는 프리팹으로 씬에 배치된 Preset1, Preest2, Preset3만 할당을 해야 합니다.
			Button은 게임 오브젝트이며 Button은 반드시 스크립트 ActiveBtn을 사용하여야 합니다.
			Button은 사용자의 마음대로 만들어도 되지만 반드시 스크립트 ActiveBtn을 포함하고 있어야 합니다.
			ActiveBtn의 DoActive를 호출하면 해당 인덱스의 리스트에 있는 슬라이딩 퍼즐이 호출이 됩니다.
		-PuzzleCamer
			내가 플레이하고 있는 퍼즐게임의 카메라를 의미합니다.
			반드시 BoardManager스크립트와 PuzzleCamerResoluitin을 포함하고 있어야 합니다.
		-ImageController
			오브젝트의 머터리얼을 내가 원하는 이미지로 적용이 가능하게 합니다.
			Texture, Sprite 두개 다 지원합니다.
			RenderType은 렌더링 모드로써 Opaue, Transparent 둘다 지원합니다.

3주의사항
	Preset이 활성화 되어 퍼즐을 섞는 동안 해당 퍼즐을 클릭하거나 비활성화하면 오류가 발생할 수 있습니다.
	Transparent는 *.jpg는 지원이 안되며 *.png는 지원이 됩니다.

English
1. Overview
	This asset is a sliding puzzle solution consisting solely of objects without any UI elements. It is primarily designed for use in Unity 3D projects.

2. Usage
	※ Note: The following layers must be added to use this asset:
		- 20: SlidingPuzzle (Default layer for sliding puzzle operation)
		- 31: StartBtn (Test layer for the activation button in the Demo scene)
		- For more details on the layers, please refer to the "PuzzleTagManager.preset" file.
	
	Prefabs

	You can place Preset1, Preset2, or Preset3 in your scene to use them.
	These objects have the following structure:
	Preset
		SlidingPuzzle
			Background
			BoardBackground
		Board
		Button
			SlidingPuzzleExitButton
				Text
			SlidingPuzzleShuffleButton
				Text
	Object Structure
	-Background
		 Background image of the puzzle, which can be customized through the ImageController.
	-BoardBackground
		 The board's background, also customizable through the ImageController.
	-Board 
		The actual game board object, which is interactive. The Size property within Board allows you to create an N x N puzzle, ranging from 2x2 to 8x8. You can specify the images for the tiles through the ImageController.
	-SlidingPuzzleExitButton
		Button to exit the puzzle game. Customizable text can be added to its child object, Text, and its image can be set in ImageController.
	-SlidingPuzzleShuffleButton 
		Button to shuffle the puzzle. Customizable text can be added to its child object, Text, and its image can be set in ImageController.
	
	Scripts
		BoardList
			It is recommended to apply this script after adding an empty project to the scene.
			Manages the puzzles placed in the scene.
			The Boards list includes elements for both Board and Button objects.
			Board should only contain the preset-prefabs (Preset1, Preset2, Preset3), while Button should be a game object that always includes the ActiveBtn script.
			Button objects can be customized but must include the ActiveBtn script.
			Calling DoActive within ActiveBtn will activate the sliding puzzle at the corresponding index in the list.
		PuzzleCamera
			Represents the camera for the puzzle game currently being played.
			Must include both the BoardManager and PuzzleCameraResolution scripts.
		ImageController
			Allows you to apply custom images to object materials.
			Supports both Texture and Sprite formats.
			RenderType allows for rendering in either Opaque or Transparent modes.
3. Caution
	Errors may occur if the puzzle is clicked or deactivated while being shuffled with an active preset.
	Note that the Transparent rendering mode supports only .png files, not .jpg files.