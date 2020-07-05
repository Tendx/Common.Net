using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Zen
{
    public abstract class ViewModelBase<TModel> where TModel : ModelBase, new()
    {
        public TModel Model { get; protected set; }
        public Window View { get; protected set; }

        protected event Action<Window> OnRendered;

        public ViewModelBase(TModel model = null) => Model = model.DeepCopy() ?? new TModel();

        public TModel Render()
        {
            View = Assembly.GetExecutingAssembly().CreateInstance(GetType().FullName.Replace("ViewModel", "View")) as Window;
            View.Owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsActive);
            View.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            View.DataContext = this;

            OnRendered?.Invoke(View);
            if (View.Owner == null)
            {
                View.Show();
                return null;
            }
            else
            {
                try
                {
                    View.Owner.Opacity = 0.9;
                    return View.ShowDialog() == true ? (View.DataContext as ViewModelBase<TModel>).Model : null;
                }
                finally
                {
                    View.Owner.Effect = null;
                    View.Owner.Opacity = 1;
                    View = null;
                }
            }
        }

        public void Confirm()
        {
            if ((bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(View))
                View.DialogResult = true;
            View.Close();
        }

        public void Leave()
        {
            if ((bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(View))
                View.DialogResult = false;
            View.Close();
        }
    }
}
