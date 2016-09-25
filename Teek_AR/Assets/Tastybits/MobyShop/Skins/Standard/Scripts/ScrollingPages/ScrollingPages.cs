/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop {


	/**
	 * The scrolling pages is a component that can be used to create a small gallery 
	 * The component is similar to a well known web component called a Slider.
	 */
	[ExecuteInEditMode]
	public class ScrollingPages : MonoBehaviour {
		public int activeIndex = -1;
		public ScrollingPagesIndicator indicator;
		public bool updateInEditMode = true;

		public GameObject scrollViewContent {
			get {
				if(scrollRect.content==null ) return null;
				return scrollRect.content.gameObject;
			}
		}

		public GameObject tempContainer; 

		public GameObject scrollViewPort;

		public UnityEngine.UI.ScrollRect scrollRect;

		public GameObject[] pageContainers;

		public GameObject pagesContainer;


		// return the inactive pages container.
		public RectTransform inactivePagesContainer {
			get {
				return pagesContainer.GetComponent<RectTransform>();
			}
		}


		public RectTransform viewPortTF {
			get {
				return scrollViewPort.GetComponent<RectTransform>();
			}
		}

		public GameObject[] pages;

		public GameObject activePage;


		Vector2 ContentLP = Vector2.zero;
		Vector2 ContentMoveSpd = Vector2.zero;

		public bool dragging=false;

		public bool brk = false;


		public bool autoScrollEnabled=false;
		public float autoScrolWaitDuration = 8f;
		public float autoScrollTime = 8f;


		void Awake() {
			if( Application.isPlaying == false ) return;
			SetupPages ();
		}


		void SetupPages() {
			inactivePagesContainer.gameObject.SetActive(false);
			int it=0;
			foreach( var p in pages ) {
				if( p == null ) {
					Debug.LogError( "Error there is a page at #" + it + " which is null.", this.gameObject );
				}
				it++;
			}
			InjectPages();
			if(indicator!=null ) {
				indicator.Clear();
				indicator.CreateItems( pages.Length );
			}
		}


		void OnEnble() {
			InjectPages();
			CorrectPageConainerPositions(false);
			autoScrollTime = autoScrolWaitDuration + 5f;
		}


		public void OnBeginDrag() {
			//var rt = scrollViewContent.GetComponent<RectTransform>();
			if(!dragging) {
				//Debug.Log("Drag begin - LocalPos : " + rt.localPosition);
			}
			CheckAutoScrollCancel();
			dragging=true;
		}


		public void OnDragMove() {
			dragging = true;
			//var rt = scrollViewContent.GetComponent<RectTransform>();
			if(dragging) {
				float vpwidth = ViewPortSize.x;
				if( vpwidth == 0f ) {
					vpwidth = 0.01f;
				}
				//float pct = rt.localPosition.x / vpwidth;
				//Debug.Log("Drag move - LocalPos : " + rt.localPosition + " viewport sz=" + ViewPortSize + " percentage = " + pct );
			}
		}


		public void OnEndDrag() {
			dragging = true;
			var rt = scrollViewContent.GetComponent<RectTransform>();
			if( dragging ) {
				float vpwidth = ViewPortSize.x;
				if( vpwidth == 0f ) {
					vpwidth = 0.01f;
				}
				float pct = rt.localPosition.x / vpwidth;
				//Debug.Log("Drag end - LocalPos : " + rt.localPosition + " viewport sz=" + ViewPortSize + " percentage = " + pct );

				if( Mathf.Abs(pct) > 0.2f ) {
					if(brk)Debug.Break();
					var lp = ContentLP;
					//var tmptf = tempContainer.transform;
			
					//RectTransform oldPageTF = pageContainers[activeIndex].GetComponent<RectTransform>();
					//var oldi = activeIndex;
					if( pct > 0f ) {
						activeIndex++;
						if( activeIndex > 2 ) 
							activeIndex = 0;
					} else {
						activeIndex--;
						if( activeIndex < 0 ) activeIndex = 2;
					}

					//	Debug.Log( "old index=" + oldi + " activeIndex=" + activeIndex );


					InjectPages();

					//Debug.Log("Change page now; activeIndex=" + activeIndex );

			

					ContentLP = lp;
					//lp.x = (Mathf.Sign(pct) * vpwidth * -1f) + ((vpwidth-(vpwidth * Mathf.Abs(pct)))*Mathf.Sign(pct));
					//rt.localPosition = lp;
				} else {
					//Debug.Log("Change page cancelled");
				}
			}
			dragging=false;
		}


		void InjectPages( ) {
			//var rt = scrollViewContent.GetComponent<RectTransform>();

			float vpwidth = ViewPortSize.x;
			if( vpwidth == 0f ) {
				vpwidth = 0.01f;
			}
			//float pct = rt.localPosition.x / vpwidth;

			int indexl = activeIndex - 1;
			int indexr = activeIndex + 1;
			if( indexl < 0 ) indexl = 2;
			if( indexr > 2 ) indexr = 0;

			int pageIdxL = activeIndex-1;
			int pageIdxR = activeIndex+1;
			if( pageIdxL < 0 ) pageIdxL = pages.Length-1;
			if( pageIdxR > pages.Length-1 ) pageIdxR = 0;

			// Get eh active page container.
			var actPageContTF = pageContainers[activeIndex].GetComponent<RectTransform>();

			// Get the tree active pages.
			var pageATF = pages[activeIndex].GetComponent<RectTransform>();
			var pageLTF = pages[pageIdxL].GetComponent<RectTransform>();
			var pageRTF = pages[pageIdxR].GetComponent<RectTransform>();


			// For all the pages set the
			for( int i=0; i<pages.Length; i++ ) {
				var pageContTF = pages[i].GetComponent<RectTransform>();
				pageContTF.SetParent( inactivePagesContainer, false );
				pageContTF.localScale = Vector3.one;
			}

			// Activate the page container.
			actPageContTF.SetParent( viewPortTF, true );

			// Page:
			//Debug.Log("setting left page container : " + actPageContTF.name + " filling in page:" + pageATF.name );
			pageATF.SetParent(actPageContTF, false);
			pageATF.anchorMin = Vector2.zero; 
			pageATF.anchorMax = Vector2.one;
			pageATF.offsetMin = pageATF.offsetMax = Vector2.zero;
			pageATF.localScale = Vector3.one;
			activePage = pageATF.gameObject;

			if( indicator!=null ){
				indicator.SetActiveIndex( activeIndex );
			}


			// Pages::
			for( int i=0; i<3; i++ ) { // update the page conainers and 
				var pageContTF = pageContainers[i].GetComponent<RectTransform>();
				if( pageContTF != actPageContTF ) {
					pageContTF.SetParent( actPageContTF, true );
					var p = pageContTF.localPosition;
					p.y = 0;
					if( i == indexl ) {
						//Debug.Log("setting left page container : " + pageContTF.name + " filling in page:" + pageLTF.name );
						p.x = vpwidth;
						pageLTF.SetParent( pageContTF, true ); 
						pageLTF.anchorMin = Vector2.zero; 
						pageLTF.anchorMax = Vector2.one; 
						pageLTF.offsetMin = pageLTF.offsetMax = Vector2.zero; 
						pageLTF.localScale = Vector3.one;
					} else if( i == indexr ) {
						//Debug.Log("setting right page container: " + pageContTF.name + " filling in page:" + pageRTF.name );
						p.x = -vpwidth;
						pageRTF.SetParent( pageContTF, true );
						pageRTF.anchorMin = Vector2.zero;
						pageRTF.anchorMax = Vector2.one;
						pageRTF.offsetMin = pageRTF.offsetMax = Vector2.zero;
						pageRTF.localScale = Vector3.one;
					}
					pageContTF.localPosition = p;
				}
			}
			scrollRect.content = actPageContTF;
		}


		Vector2 ViewPortSize {
			get {
				var rt2 = scrollViewPort.GetComponent<RectTransform>();
				Vector3[] corners = new Vector3[4];
				rt2.GetLocalCorners(corners);
				var size = corners[2] - corners[0];
				if( (float)size.x == 0 ) {
					//Debug.LogError("Error getting RT size");
					return rt2.sizeDelta;
				}
				return size;
			}
		}


		// Check that the page containers are with the right offsets
		void CorrectPageConainerPositions( bool errorMsg=true ) {
			//var rt = scrollViewContent.GetComponent<RectTransform>();

			float vpwidth = ViewPortSize.x;
			if( vpwidth == 0f ) {
				vpwidth = 0.01f;
			}
			if( vpwidth < 1f) {
				Debug.LogError( "Error viewport size is zero or close to zero!" );
			}

			int indexl = activeIndex - 1;
			int indexr = activeIndex + 1;
			if( indexl < 0 ) indexl = 2;
			if( indexr > 2 ) indexr = 0;

			var actPageContTF = pageContainers[activeIndex].GetComponent<RectTransform>();

			for( int i=0; i<3; i++ ) { // update the page conainers and 
				var pageContTF = pageContainers[i].GetComponent<RectTransform>();
				if( pageContTF != actPageContTF ) {
					pageContTF.SetParent( actPageContTF, true );
					var p = pageContTF.localPosition;
					p.y = 0;
					var ax = Mathf.Abs(p.x);
					var aw = Mathf.Abs(vpwidth);
					//var diff = Mathf.Max( ax, aw ) - Mathf.Min( ax, aw );
					if( ax < 3f && aw > 1 ) {
						if(errorMsg)
							Debug.Log( "The position of the pagecontainer is not offset correctly position=" + p.x, pageContTF.gameObject );
					}
					if( i == indexl ) {
						p.x = vpwidth;
					} else if( i == indexr ) {
						p.x = -vpwidth;
					}
					pageContTF.localPosition = p;
				}
			}
		}

		public float autoScrollPct = 0.21f;
		public float autoScrollSpd = 0.3f;
		public bool autoScrollNow=false;


		/**
		 * Rest the pages and make it show sometihng diffirent,
		 */
		public void SetPages( GameObject[] _pages, int activeIndex ) {
			if( MobyShop.Shop.Verbose ) Debug.Log ("Set pages #" + _pages.Length +  " active index = " + activeIndex );
			foreach (var p in pages) {
				p.transform.SetParent( pagesContainer.transform );
			}
			this.activeIndex = activeIndex;
			this.pages = _pages;
			autoScrollTime = autoScrolWaitDuration;
			SetupPages ();
		}


		/**
		 *  Begin the Auto Scroll.
		 */
		void BeginAutoScroll() {
			//Debug.Log("AutoScroll Begin");
			autoScrollNow=true;
			this.scrollRect.movementType = UnityEngine.UI.ScrollRect.MovementType.Unrestricted;
		}


		void CheckAutoScrollCancel() {
			if( dragging && autoScrollNow ) {
				autoScrollNow = false;
				//Debug.Log("Autoscroll cancelled");
			}
		}


		Vector2 DragAutoScroll( RectTransform rt, Vector2 lp ) {
			float vpwidth = ViewPortSize.x;
			if( vpwidth == 0f ) {
				vpwidth = 0.01f;
			}
			// liniear autoscroll
			if(autoScrollSpd<=0.1f)autoScrollSpd=0.1f;
			var move =  (vpwidth*autoScrollPct) * 1.0f/*move move is 1 percent...*/ * (Time.deltaTime / autoScrollSpd);
			lp.x += move;
			//Debug.Log("AutoScroll Drag; move=" + move + "; width=" + vpwidth + " lp = " + lp );

			
			rt.localPosition = lp;
			if( lp.x >= vpwidth*autoScrollPct ) {
				EndAutoScroll();
			}

			return lp;
		}


		void EndAutoScroll() {
			//Debug.Log("AutoScroll End");  
			autoScrollNow=false;
			this.scrollRect.movementType = UnityEngine.UI.ScrollRect.MovementType.Elastic;
			OnEndDrag(  ); 
		}


		void Update () {
			var rt = scrollViewContent.GetComponent<RectTransform>();
			if( Application.isPlaying ) {

				var lp = rt.localPosition;
				ContentMoveSpd = (ContentLP - new Vector2(lp.x, lp.y))*Time.deltaTime;

				if( autoScrollNow ) {
					lp = DragAutoScroll( rt, lp );
				}
				ContentLP = lp;

				

				

				

				if( autoScrollEnabled ) {
					autoScrollTime -= Time.deltaTime;
					if( autoScrollTime < 0f ){
						BeginAutoScroll();
						autoScrollTime = autoScrolWaitDuration;
					}
					if( ContentMoveSpd.magnitude > 0.1f ) { // reset...
						//Debug.Log("Resetting autoscrolling counter; DragDelta:" + ContentMoveSpd );
						autoScrollTime = autoScrolWaitDuration;
					}
				}



				CorrectPageConainerPositions( false );
			}
	#if UNITY_EDITOR
			if( Application.isEditor && Application.isPlaying == false && updateInEditMode ) {
				/*var items = this.gameObject.GetComponentsInChildren<ScrollingPagesItem>();
				foreach( var item in items ) {
					
				}*/
			}
	#endif
		}

	}


}
