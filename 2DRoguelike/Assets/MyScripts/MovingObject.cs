using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float movingSpeed = 0.1f;
	public LayerMask blocklingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2D = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / movingSpeed;
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit){
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blocklingLayer);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;
	}

	protected IEnumerator SmoothMovement(Vector3 end){
		float remainingDistance = (transform.position - end).sqrMagnitude;
		while (remainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition (newPosition);
			remainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}
		
	protected virtual void AttemptMove<T>(int xDir, int yDir) where T: Component{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);
		if (hit.transform == null) {
			return;
		}
		T hitComponent = hit.transform.GetComponent<T> ();
		if (!canMove && hitComponent != null) {
			OnCantMove (hitComponent);
		}		
	}

	protected abstract void OnCantMove<T> (T component)
		where T: Component;
}
