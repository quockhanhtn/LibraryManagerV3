using System.Windows;

namespace LibraryManager.EntityFramework.View
{
   /// <summary>
   /// Interaction logic for AddBookWindow.xaml
   /// </summary>
   public partial class AddBookWindow : Window
   {
      public AddBookWindow()
      {
         InitializeComponent();
         txtTitle.Focus();
      }
   }
}