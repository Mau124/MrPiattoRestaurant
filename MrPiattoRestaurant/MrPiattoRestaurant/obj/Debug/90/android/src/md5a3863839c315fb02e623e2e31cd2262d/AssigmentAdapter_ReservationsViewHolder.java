package md5a3863839c315fb02e623e2e31cd2262d;


public class AssigmentAdapter_ReservationsViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.adapters.AssigmentAdapter+ReservationsViewHolder, MrPiattoRestaurant", AssigmentAdapter_ReservationsViewHolder.class, __md_methods);
	}


	public AssigmentAdapter_ReservationsViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == AssigmentAdapter_ReservationsViewHolder.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.adapters.AssigmentAdapter+ReservationsViewHolder, MrPiattoRestaurant", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
