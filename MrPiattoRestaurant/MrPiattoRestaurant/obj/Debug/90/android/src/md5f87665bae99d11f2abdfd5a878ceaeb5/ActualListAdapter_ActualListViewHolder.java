package md5f87665bae99d11f2abdfd5a878ceaeb5;


public class ActualListAdapter_ActualListViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.actualListAdapters.ActualListAdapter+ActualListViewHolder, MrPiattoRestaurant", ActualListAdapter_ActualListViewHolder.class, __md_methods);
	}


	public ActualListAdapter_ActualListViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == ActualListAdapter_ActualListViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.actualListAdapters.ActualListAdapter+ActualListViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
