using System.Windows;

namespace LibraryManager.EntityFramework.View
{
   /// <summary>
   /// Interaction logic for AddMemberWindow.xaml
   /// </summary>
   public partial class AddMemberWindow : Window
   {
      public AddMemberWindow()
      {
         InitializeComponent();
         txtLastName.Focus();
      }
   }
}