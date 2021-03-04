using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleOutline.Views
{
    public class TextInputDialog
    {
        public string Message { get; set; } = String.Empty;
        public string InputText { get; set; } = String.Empty;
        public string Title { get; set; } = "Text Input Form";
        DialogResult Result { get; set; } = DialogResult.None;

        public DialogResult ShowDialog()
        {
            var textInputForm = new TextInputForm();
            textInputForm.Text = Title;
            textInputForm.Message = Message;
            textInputForm.InputText = InputText;

            Result = textInputForm.ShowDialog();

            InputText = textInputForm.InputText;

            return Result;
        }

        public static string PromptUser(string message, string title, string input)
        {
            var dialog = new TextInputDialog()
            {
                Message = message,
                Title = title,
                InputText = input
            };

            dialog.ShowDialog();

            if (dialog.Result == DialogResult.OK)
            {
                return dialog.InputText;
            }

            return null;
        }
    }
}
