package md5a3863839c315fb02e623e2e31cd2262d;


public class SectionedExpandableGridAdapter_TableViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.SectionedExpandableGridAdapter+TableViewHolder, MrPiattoRestaurant", SectionedExpandableGridAdapter_TableViewHolder.class, __md_methods);
	}


	public SectionedExpandableGridAdapter_TableViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == SectionedExpandableGridAdapter_TableViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.SectionedExpandableGridAdapter+TableViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
