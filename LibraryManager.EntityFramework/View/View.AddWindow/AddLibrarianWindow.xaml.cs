using System.Windows;

namespace LibraryManager.EntityFramework.View
{
   /// <summary>
   /// Interaction logic for AddLibrarianWindow.xaml
   /// </summary>
   public partial class AddLibrarianWindow : Window
   {
      public AddLibrarianWindow()
      {
         InitializeComponent();
         txtLastName.Focus();
      }
   }
}