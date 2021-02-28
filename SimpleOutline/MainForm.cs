//Unfinished Project

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleOutline
{

    public partial class MainForm : Form
    {
        private OutlineDocumentsController outlineDocumentsController;
        private OutlineDocuments outlineDocuments;
        public MainForm()
        {
            InitializeComponent();

            InitOutlineDocuments();
        }

        public void InitOutlineDocuments()
        {
            ctrlOutlinesTabView.TabPages.Clear();

            outlineDocuments = new OutlineDocuments();
            outlineDocumentsController = new OutlineDocumentsController(ctrlOutlinesTabView, outlineDocuments);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.AddNewDocument();
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {

        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {

        }

        private void menuItemSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void menuItemPrint_Click(object sender, EventArgs e)
        {

        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {

        }

        private void menuItemUndo_Click(object sender, EventArgs e)
        {

        }

        private void menuItemRedo_Click(object sender, EventArgs e)
        {

        }

        private void menuItemInsert_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.InsertNewOutlineItemIntoSelectedDocument();
        }

        private void menuItemRename_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.RenameSelectedOutlineItemInSelectedDocument();
        }

        private void menuItemMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void menuItemMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void menuItemCut_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.CutSelectedOutlineItemInSelectedDocument();
        }

        private void menuItemCopy_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.CopySelectedOutlineItemInSelectedDocument();
        }

        private void menuItemPaste_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.PasteOutlineItemInClipboardInSelectedDocument();
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            outlineDocumentsController.DeleteSelectedOutlineItemFromSelectedDocument();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {

        }
    }
}
