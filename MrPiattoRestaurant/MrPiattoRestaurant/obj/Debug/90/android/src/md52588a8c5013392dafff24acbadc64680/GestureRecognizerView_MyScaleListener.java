package md52588a8c5013392dafff24acbadc64680;


public class GestureRecognizerView_MyScaleListener
	extends android.view.ScaleGestureDetector.SimpleOnScaleGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScale:(Landroid/view/ScaleGestureDetector;)Z:GetOnScale_Landroid_view_ScaleGestureDetector_Handler\n" +
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.GestureRecognizerView+MyScaleListener, MrPiattoRestaurant", GestureRecognizerView_MyScaleListener.class, __md_methods);
	}


	public GestureRecognizerView_MyScaleListener ()
	{
		super ();
		if (getClass () == GestureRecognizerView_MyScaleListener.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.GestureRecognizerView+MyScaleListener, MrPiattoRestaurant", "", this, new java.lang.Object[] {  });
	}

	public GestureRecognizerView_MyScaleListener (md52588a8c5013392dafff24acbadc64680.GestureRecognizerView p0)
	{
		super ();
		if (getClass () == GestureRecognizerView_MyScaleListener.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.GestureRecognizerView+MyScaleListener, MrPiattoRestaurant", "MrPiattoRestaurant.GestureRecognizerView, MrPiattoRestaurant", this, new java.lang.Object[] { p0 });
	}


	public boolean onScale (android.view.ScaleGestureDetector p0)
	{
		return n_onScale (p0);
	}

	private native boolean n_onScale (android.view.ScaleGestureDetector p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
