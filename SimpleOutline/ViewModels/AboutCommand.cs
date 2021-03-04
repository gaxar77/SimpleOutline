namespace SimpleOutline.ViewModels
{
    public class AboutCommand : CommandBase
    {
        public AboutCommand(ViewModel1 viewModel)
            : base(viewModel)
        {

        }

        public override void Execute(object parameter)
        {
            var aboutMessage = "Simple Outline 1.0 - Created by Guido Arbia (g.arbia777@gmail.com)";

            System.Windows.MessageBox.Show(aboutMessage, "About Simple Outline 1.0");
        }
    }
}
