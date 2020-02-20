package md5fe7364dc2710670472cef6c41ba85e72;


public class WaitFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("MrPiattoRestaurant.Fragments.Reservations.WaitFragment, MrPiattoRestaurant", WaitFragment.class, __md_methods);
	}


	public WaitFragment ()
	{
		super ();
		if (getClass () == WaitFragment.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.Fragments.Reservations.WaitFragment, MrPiattoRestaurant", "", this, new java.lang.Object[] {  });
	}

	public WaitFragment (android.content.Context p0)
	{
		super ();
		if (getClass () == WaitFragment.class)
			mono.android.TypeManager.Activate ("MrPiattoRestaurant.Fragments.Reservations.WaitFragment, MrPiattoRestaurant", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);

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
