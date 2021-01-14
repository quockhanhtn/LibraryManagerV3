using System.Windows;

namespace LibraryManager.EntityFramework.View
{
   /// <summary>
   /// Interaction logic for AddPublisherWindow.xaml
   /// </summary>
   public partial class AddPublisherWindow : Window
   {
      public AddPublisherWindow()
      {
         InitializeComponent();
         txtName.Focus();
      }
   }
}