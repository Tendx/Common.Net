using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using LE = System.Linq.Expressions;

namespace Zen
{
    class Router
    {
        public string Path { get; set; }
        public DependencyObject Sender { get; set; }
        public object Via { get; set; }
        public object Redirect { get; set; }

        public void Build()
        {
            Window owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsLoaded == false);
            if (string.IsNullOrEmpty(Path) || owner == null) return;
            string[] nodes = Path.Split(new[] { "->" }, StringSplitOptions.None);
            if (nodes.Length != 2) return;
            string eName = nodes[0];
            string mName = nodes[1];
            owner.DataContextChanged += (s, e) =>
            {
                if (e.OldValue != null) return;

                LE.MemberExpression via = LE.Expression.Property(LE.Expression.Constant(this), GetType().GetProperty("Via"));
                object dst = Redirect ?? e.NewValue;
                MethodInfo mi = dst.GetType().GetMethod(mName, BindingFlags.Public | BindingFlags.Instance);
                EventInfo ei = Sender.GetType().GetEvent(eName, BindingFlags.Public | BindingFlags.Instance);

                LE.ParameterExpression[] src_params = ei.EventHandlerType.GetMethod("Invoke").GetParameters().Select(p => LE.Expression.Parameter(p.ParameterType)).ToArray();
                LE.Expression[] dst_params = new LE.Expression[mi.GetParameters().Length];

                for (int i = 0; i < dst_params.Length && i < src_params.Length; i++)
                    dst_params[i] = i == 0 ? LE.Expression.Coalesce(via, src_params[0]) as LE.Expression : src_params[i] as LE.Expression;

                LE.Expression body = LE.Expression.Call(LE.Expression.Constant(dst), mi, dst_params);
                LE.LambdaExpression lambda = LE.Expression.Lambda(body, src_params);

                ei.AddEventHandler(Sender, Delegate.CreateDelegate(ei.EventHandlerType, lambda.Compile(), "Invoke", false));
            };
        }
    }
}
