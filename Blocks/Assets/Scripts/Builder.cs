using UnityEngine;
using UnityEngine.UI;

public class Builder : MonoBehaviour {
	public float buildDistance;
	public InventoryBlock[] blocks;
	private int selectedBlock = 0;
	public Renderer blockUI;
	public Text blockAmount;
	public Renderer[] blockUILeft;
	public Renderer[] blockUIRight;
	public Renderer blockHover;

	private void Awake() {
		updateBlockUI();
		blockHover.enabled = false;
	}

	private void Update() {
		Ray ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, buildDistance)) {
			Block block = hit.transform.GetComponent<Block>();
			if (block) {
				Vector3 newBlockPosition = showBlockHover(hit.transform.position, hit.point);
				if (Input.GetMouseButtonDown(0)) {
					addBlock(block.id);
					block.breakBlock();
				} else if (Input.GetMouseButtonDown(1) && blocks[selectedBlock].amount > 0) {
					blocks[selectedBlock].amount--;
					blockAmount.text = blocks[selectedBlock].amount.ToString();
					GameObject newBlock = Instantiate(blocks[selectedBlock].block.gameObject, newBlockPosition, hit.transform.rotation);
					newBlock.GetComponent<Block>().placeBlock();
				}
			} else {
				blockHover.enabled = false;
			}
		} else {
			blockHover.enabled = false;
		}

		if (Input.GetKeyDown("q") || Input.mouseScrollDelta.y < 0.0f) {
			selectedBlock--;
			if (selectedBlock < 0) {
				selectedBlock = blocks.Length - 1;
			}
			updateBlockUI();
		} else if (Input.GetKeyDown("e") || Input.mouseScrollDelta.y > 0.0f) {
			selectedBlock++;
			if (selectedBlock >= blocks.Length) {
				selectedBlock = 0;
			}
			updateBlockUI();
		}
	}

	private void addBlock(string id) {
		for (int index = 0; index < blocks.Length; index++) {
			if (blocks[index].block.id == id) {
				blocks[index].amount++;
				blockAmount.text = blocks[selectedBlock].amount.ToString();
				return;
			}
		}
		Debug.LogError("Unknown block ID: " + id);
	}

	private Vector3 showBlockHover(Vector3 blockPosition, Vector3 hitPosition) {
		blockHover.enabled = true;
		Vector3 newBlockPosition = blockPosition;
		Vector3 hoverPosition = blockPosition;
		Vector3 difference = blockPosition - hitPosition;
		Vector3 distance = new Vector3(Mathf.Abs(difference.x), Mathf.Abs(difference.y), Mathf.Abs(difference.z));
		if (distance.x > distance.y && distance.x > distance.z) {
			blockHover.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
			if (difference.x < 0.0f) {
				newBlockPosition.x += 1.0f;
				hoverPosition.x += 0.505f;
			} else {
				newBlockPosition.x -= 1.0f;
				hoverPosition.x -= 0.505f;
			}
		} else if (distance.y > distance.z) {
			blockHover.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			if (difference.y < 0.0f) {
				newBlockPosition.y += 1.0f;
				hoverPosition.y += 0.505f;
			} else {
				newBlockPosition.y -= 1.0f;
				hoverPosition.y -= 0.505f;
			}
		} else {
			blockHover.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			if (difference.z < 0.0f) {
				newBlockPosition.z += 1.0f;
				hoverPosition.z += 0.505f;
			} else {
				newBlockPosition.z -= 1.0f;
				hoverPosition.z -= 0.505f;
			}
		}
		blockHover.transform.position = hoverPosition;
		return newBlockPosition;
	}

	private void updateBlockUI() {
		blockUI.material = blocks[selectedBlock].block.GetComponent<Renderer>().sharedMaterial;
		blockAmount.text = blocks[selectedBlock].amount.ToString();
		int blockIndex = selectedBlock;
		for (int index = 0; index < blockUILeft.Length; index++) {
			blockIndex--;
			if (blockIndex < 0) {
				blockIndex = blocks.Length - 1;
			}
			blockUILeft[index].material = blocks[blockIndex].block.GetComponent<Renderer>().sharedMaterial;
		}
		blockIndex = selectedBlock;
		for (int index = 0; index < blockUIRight.Length; index++) {
			blockIndex++;
			if (blockIndex >= blocks.Length) {
				blockIndex = 0;
			}
			blockUIRight[index].material = blocks[blockIndex].block.GetComponent<Renderer>().sharedMaterial;
		}
	}

	[System.Serializable]
	public struct InventoryBlock {
		public Block block;
		public int amount;
	}
}
