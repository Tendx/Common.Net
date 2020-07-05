using System.Windows;

namespace Zen
{
    public class Route
    {
        #region Path
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.RegisterAttached("Path", typeof(string), typeof(Route),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnPathChanged)));

        public static void SetPath(DependencyObject d, string value) => d.SetValue(PathProperty, value);

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Router router = GetRouter(d);
            router.Sender = d;
            router.Path = e.NewValue?.ToString().Replace(" ", "");
            router.Build();
        }
        #endregion

        #region Via
        public static readonly DependencyProperty ViaProperty =
            DependencyProperty.RegisterAttached("Via", typeof(object), typeof(Route),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnViaChanged)));

        public static void SetVia(DependencyObject d, object value) => d.SetValue(ViaProperty, value);

        private static void OnViaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Router router = GetRouter(d);
            router.Via = e.NewValue;
        }
        #endregion

        #region Redirect
        public static readonly DependencyProperty RedirectProperty =
            DependencyProperty.RegisterAttached("Redirect", typeof(object), typeof(Route),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnRedirectChanged)));

        public static void SetRedirect(DependencyObject d, object value) => d.SetValue(RedirectProperty, value);

        private static void OnRedirectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Router router = GetRouter(d);
            router.Redirect = e.NewValue;
        }
        #endregion

        #region Router
        private static readonly DependencyProperty RouterProperty =
            DependencyProperty.RegisterAttached("Router", typeof(Router), typeof(Route), new FrameworkPropertyMetadata());

        private static Router GetRouter(DependencyObject d)
        {
            Router router = d.GetValue(RouterProperty) as Router;
            if (router is null)
            {
                router = new Router();
                SetRouter(d, router);
            }
            return router;
        }

        private static void SetRouter(DependencyObject d, Router router) =>
            d.SetValue(RouterProperty, router);
        #endregion
    }
}
