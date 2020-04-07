package md5a3863839c315fb02e623e2e31cd2262d;


public class WaitersAdapter_WaitersViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.WaitersAdapter+WaitersViewHolder, MrPiattoRestaurant", WaitersAdapter_WaitersViewHolder.class, __md_methods);
	}


	public WaitersAdapter_WaitersViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == WaitersAdapter_WaitersViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.WaitersAdapter+WaitersViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
