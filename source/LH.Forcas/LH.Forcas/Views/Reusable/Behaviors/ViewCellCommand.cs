namespace LH.Forcas.Views.Reusable.Behaviors
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public static class ViewCellCommand
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(
          "Command",
          typeof(ICommand),
          typeof(ViewCellCommand),
          null,
          propertyChanged: HandleCommandChanged);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached(
          "CommandParameter",
          typeof(object),
          typeof(ViewCellCommand),
          null);

        public static ICommand GetCommand(BindableObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(BindableObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }

        private static void HandleCommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewCell = (ViewCell)bindable;

            if (newvalue is ICommand)
            {
                viewCell.Tapped += ExecuteCommand;
            }
            else
            {
                viewCell.Tapped -= ExecuteCommand;
            }
        }

        private static void ExecuteCommand(object sender, EventArgs eventArgs)
        {
            var bindable = (BindableObject) sender;

            var command = GetCommand(bindable);
            var parameter = GetCommandParameter(bindable);

            command?.Execute(parameter);
        }
    }
}