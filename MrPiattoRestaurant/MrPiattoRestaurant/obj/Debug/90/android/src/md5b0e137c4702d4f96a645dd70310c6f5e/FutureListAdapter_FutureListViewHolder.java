package md5b0e137c4702d4f96a645dd70310c6f5e;


public class FutureListAdapter_FutureListViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.futureListAdapters.FutureListAdapter+FutureListViewHolder, MrPiattoRestaurant", FutureListAdapter_FutureListViewHolder.class, __md_methods);
	}


	public FutureListAdapter_FutureListViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == FutureListAdapter_FutureListViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.futureListAdapters.FutureListAdapter+FutureListViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
