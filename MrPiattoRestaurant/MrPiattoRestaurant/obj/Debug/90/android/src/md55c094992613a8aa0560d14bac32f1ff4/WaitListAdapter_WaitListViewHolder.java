package md55c094992613a8aa0560d14bac32f1ff4;


public class WaitListAdapter_WaitListViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.waitListAdapters.WaitListAdapter+WaitListViewHolder, MrPiattoRestaurant", WaitListAdapter_WaitListViewHolder.class, __md_methods);
	}


	public WaitListAdapter_WaitListViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == WaitListAdapter_WaitListViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.waitListAdapters.WaitListAdapter+WaitListViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
