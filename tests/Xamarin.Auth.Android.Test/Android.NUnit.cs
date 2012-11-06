using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Reflection;
using Android.Graphics;
using System.IO;

namespace Android.NUnit
{
	[Activity (Label = "TestRunner", MainLauncher = true)]
	public class TestRunner : ListActivity
	{
		public static TestRunner Shared { get; private set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Shared = this;

			var asm = Assembly.GetExecutingAssembly ();
			Title = asm.GetName ().Name;

			ListAdapter = new FixtureAdapter (
				this,
				from t in asm.GetTypes ()
				where t.GetCustomAttributes (typeof (global::NUnit.Framework.TestFixtureAttribute), true).Length > 0
				orderby t.FullName
				select t);

			ListView.ItemClick += (s, e) => {
				var f = ((FixtureAdapter)ListAdapter)[e.Position];
				var i = new Intent (this, typeof(FixtureActivity));
				i.PutExtra ("FixtureFullName", f.FullName);
				StartActivity (i);
			};
		}

		class FixtureAdapter : BaseAdapter
		{
			TestRunner activity;
			List<Type> fixtures;

			public FixtureAdapter (TestRunner activity, IEnumerable<Type> fixtures)
			{
				this.activity = activity;
				this.fixtures = fixtures.ToList ();
			}

			public Type this[int position] { get { return fixtures[position]; } }

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return 0;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				var layout = convertView as LinearLayout;
				if (layout == null) {
					var ntv = new TextView (activity) {
						Id = 42,
						LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent) {
							TopMargin = 12,
							BottomMargin = 12,
						},
					};
					ntv.SetTextColor (Color.White);
					ntv.SetTextSize (Android.Util.ComplexUnitType.Sp, 20);
					layout = new LinearLayout (activity);
					layout.AddView (ntv);
				}
				var item = fixtures[position];
				var tv = layout.FindViewById<TextView> (42);
				tv.Text = item.Name;
				return layout;
			}

			public override int Count {
				get {
					return fixtures.Count;
				}
			}
		}
	}

	[Activity (Label = "Fixture", MainLauncher = true)]			
	public class FixtureActivity : ListActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			var fixture = Assembly.GetExecutingAssembly ().GetType (Intent.GetStringExtra ("FixtureFullName"));

			Title = fixture.Name;

			ListAdapter = new TestAdapter (
				this,
				from t in fixture.GetMethods ()
				where t.GetCustomAttributes (typeof (global::NUnit.Framework.TestAttribute), true).Length > 0
				orderby t.Name
				select t);

			ListView.ItemClick += (s, e) => {
				RunTest (((TestAdapter)ListAdapter)[e.Position]);
			};
		}

		void RunTest (MethodInfo test)
		{
			try {
				var o = Activator.CreateInstance (test.DeclaringType);
				test.Invoke (o, null);
			}
			catch (TargetInvocationException ex) {
				ShowError (ex.InnerException);
			}
		}

		void ShowError (Exception ex)
		{
			var b = new AlertDialog.Builder (this);
			b.SetMessage (ex.ToString ());
			b.SetTitle (ex.GetType ().Name);
			b.SetNeutralButton ("OK", (s, e) => {
				((AlertDialog)s).Cancel ();
			});
			var alert = b.Create ();
			alert.Show ();
		}

		class TestAdapter : BaseAdapter
		{
			FixtureActivity activity;
			List<MethodInfo> tests;

			public TestAdapter (FixtureActivity activity, IEnumerable<MethodInfo> tests)
			{
				this.activity = activity;
				this.tests = tests.ToList ();
			}

			public MethodInfo this[int position] { get { return tests[position]; } }

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return 0;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				var layout = convertView as LinearLayout;
				if (layout == null) {
					var ntv = new TextView (activity) {
						Id = 42,
						LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent) {
							TopMargin = 12,
							BottomMargin = 12,
						},
					};
					ntv.SetTextColor (Color.White);
					ntv.SetTextSize (Android.Util.ComplexUnitType.Sp, 20);
					layout = new LinearLayout (activity);
					layout.AddView (ntv);
				}
				var item = tests[position];
				var tv = layout.FindViewById<TextView> (42);
				tv.Text = item.Name;
				return layout;
			}

			public override int Count {
				get {
					return tests.Count;
				}
			}
		}
	}
}

namespace NUnit.Framework
{
	public class TestFixtureAttribute : Attribute
	{
	}

	public class TestAttribute : Attribute
	{
	}

	public class Assert
	{
		public static void That (bool condition)
		{
			if (!condition) {
				throw new AssertionException ("<true>");
			}
		}

		public static void That (object actual, IResolveConstraint constraint)
		{
			if (!constraint.Matches (actual)) {
				var s = new StringWriter ();
				constraint.WriteMessageTo (s);
				throw new AssertionException (s.ToString ());
			}
		}
	}

	public static class Is
	{
		public static IResolveConstraint EqualTo (object expected)
		{
			return new EqualConstraint (expected);
		}
		public static IResolveConstraint Null ()
		{
			return new NullConstraint ();
		}
		public static class Not
		{
			public static IResolveConstraint EqualTo (object expected)
			{
				return new NotConstraint (new EqualConstraint (expected));
			}
			public static IResolveConstraint Null ()
			{
				return new NotConstraint (new NullConstraint ());
			}
		}
	}

	public interface IResolveConstraint
	{
		bool Matches (object actual);
		void WriteMessageTo (TextWriter writer);
	}

	public abstract class Constraint : IResolveConstraint
	{
		protected object actual;
		public abstract bool Matches (object actual);
		public abstract void WriteMessageTo (TextWriter writer);
	}

	public class EqualConstraint : Constraint
	{
		object expected;
		public EqualConstraint (object expected)
		{
			this.expected = expected;
		}
		public override bool Matches (object actual)
		{
			this.actual = actual;
			return expected.Equals (actual);
		}
		public override void WriteMessageTo (TextWriter writer)
		{
			writer.Write ("<{0}> == <{1}>", actual, expected);
		}
	}

	public class NullConstraint : Constraint
	{
		public override bool Matches (object actual)
		{
			return actual == null;
		}
		public override void WriteMessageTo (TextWriter writer)
		{
			writer.Write ("<{0}> == NULL", actual);
		}
	}

	public class NotConstraint : Constraint
	{
		Constraint constraint;
		public NotConstraint (Constraint constraint)
		{
			this.constraint = constraint;
		}
		public override bool Matches (object actual)
		{
			return !constraint.Matches (actual);
		}
		public override void WriteMessageTo (TextWriter writer)
		{
			writer.Write ("NOT ");
			constraint.WriteMessageTo (writer);
		}
	}

	public class AssertionException : Exception
	{
		public AssertionException (string message)
			: base (message)
		{
		}
	}
}

